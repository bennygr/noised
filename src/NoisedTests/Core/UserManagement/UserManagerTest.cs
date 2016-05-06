using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Noised.Core.DB;
using Noised.Core.UserManagement;
using Should;

namespace NoisedTests.Core.UserManagement
{
    [TestFixture]
    public class UserManagerTest
    {
        private UserManager uMan;
        private Mock<IUserRepository> mockUserRepository;
        private List<User> userList;

        [TestFixtureSetUp]
        public void OneTimeSetup()
        {
            userList = new List<User>();

            //Mocking a user repoitory which operatores on an in-memory list
            mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(x => x.CreateUser(It.IsAny<User>())).Callback(CreateUserAction());
            mockUserRepository.Setup(x => x.GetUser(It.IsAny<String>())).Returns(GetUserFunc());
            mockUserRepository.Setup(x => x.DeleteUser(It.IsAny<User>())).Callback(DeleteUserAction());
            mockUserRepository.Setup(x => x.UpdateUser(It.IsAny<User>())).Callback(new Action<User>(UpdateUserlistAction));

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.UserRepository).Returns(mockUserRepository.Object);

            var mockFactory = new Mock<IDbFactory>();
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
            return userList.Add;
        }

        private void UpdateUserlistAction(User user)
        {
            DeleteUserAction();
            CreateUserAction();
        }
        
        [Test]
        public void CreateUser()
        {
            const string userName = "username";
            const string password = "password";
            
            //Testing the creation of a user
            uMan.CreateUser(userName,password);

            //Assert that the usermanager has stored a user with the given name into the repo
            mockUserRepository.Verify(r => r.CreateUser(It.Is<User>(u => u.Name == userName)));

            //Check if the user was created
            var user = uMan.GetUser(userName);
            user.ShouldNotBeNull();
            user.Name.ShouldEqual(userName);
        }

        //[Test]
        public void AuthenticateUser()
        {
            uMan.CreateUser("authTestUser", "authTestPassword");
            uMan.Authenticate("authTestUser", "authTestPassword").ShouldBeTrue("This user should authenticate correctly.");
            uMan.Authenticate("authTestUser", "authTestPassword2").ShouldBeFalse("This user should not authenticate correctly.");
            uMan.Authenticate("authTestUser2", "authTestPassword").ShouldBeFalse("This user should not authenticate correctly.");
        }

        //[Test]
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
