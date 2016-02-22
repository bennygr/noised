using System;
using System.Collections.Generic;
using Moq;
using Noised.Core.DB;
using Noised.Core.IOC;
using Noised.Core.UserManagement;
using NUnit.Framework;
using Should;

namespace NoisedTests.Core.UserManagement
{
    [TestFixture]
    public class UserManagerTest
    {
        private UserManager uMan;
        private List<User> userList;

        [TestFixtureSetUp]
        public void OneTimeSetup()
        {
            userList = new List<User>();
            IocContainer.Build();

            Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(x => x.CreateUser(It.IsAny<User>())).Callback(CreateUserAction());
            mockUserRepository.Setup(x => x.GetUser(It.IsAny<String>())).Returns(GetUserFunc());
            mockUserRepository.Setup(x => x.DeleteUser(It.IsAny<User>())).Callback(DeleteUserAction());
            mockUserRepository.Setup(x => x.UpdateUser(It.IsAny<User>())).Callback(new Action<User>(UpdateUserlistAction));

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.UserRepository).Returns(mockUserRepository.Object);

            Mock<IDbFactory> mockFactory = new Mock<IDbFactory>();
            mockFactory.Setup(x => x.GetUnitOfWork()).Returns(mockUnitOfWork.Object);

            uMan = new UserManager(mockFactory.Object);
        }

        private Func<string, User> GetUserFunc()
        {
            return (s => userList.Find(x => x.Name == s));
        }

        private Action<User> DeleteUserAction()
        {
            return s => userList.Remove(userList.Find(y => y.Name == s.Name));
        }

        private Action<User> CreateUserAction()
        {
            return user => userList.Add(user);
        }

        private void UpdateUserlistAction(User user)
        {
            DeleteUserAction();
            CreateUserAction();
        }

        [Test]
        public void CreateUser()
        {
            uMan.CreateUser("test", "user");
        }

        [Test]
        public void AuthenticateUser()
        {
            uMan.CreateUser("authTestUser", "authTestPassword");
            uMan.Authenticate("authTestUser", "authTestPassword").ShouldBeTrue("This user should authenticate correctly.");
            uMan.Authenticate("authTestUser", "authTestPassword2").ShouldBeFalse("This user should not authenticate correctly.");
            uMan.Authenticate("authTestUser2", "authTestPassword").ShouldBeFalse("This user should not authenticate correctly.");
        }

        [Test]
        public void DeleteUser()
        {
            uMan.CreateUser("deleteTestUser", "deleteTestPassword");
            uMan.GetUser("deleteTestUser").ShouldNotBeNull("The user should exist.");
            uMan.DeleteUser(new User("deleteTestUser"));
            uMan.GetUser("deleteTestUser").ShouldEqual(null, "The User should no longer exist.");
        }

        [Test]
        public void UpdateUser()
        {
            uMan.CreateUser("updateTestUser1", "deleteTestPassword");

            User u = uMan.GetUser("updateTestUser1");
            u.ShouldNotBeNull("updateTestUser1 was just created. Should not be null.");
            u.Name = "updateTestUser2";

            uMan.UpdateUser(u);
            uMan.GetUser("updateTestUser1").ShouldEqual(null, "updateTestUser1 is now updateTestUser2 and therefore should not be found with updateTestUser1.");
            uMan.Authenticate("updateTestUser2", "deleteTestPassword").ShouldBeTrue("After changing the Name we should still be able to authenticate.");
        }
    }
}
