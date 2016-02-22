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
            mockUserRepository.Setup(x => x.CreateUser(It.IsAny<User>())).Callback((User user) => userList.Add(user));
            mockUserRepository.Setup(x => x.GetUser(It.IsAny<String>()))
                .Returns(new Func<string, User>(s => userList.Find(x => x.Name == s)));

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.UserRepository).Returns(mockUserRepository.Object);

            Mock<IDbFactory> mockFactory = new Mock<IDbFactory>();
            mockFactory.Setup(x => x.GetUnitOfWork()).Returns(mockUnitOfWork.Object);

            uMan = new UserManager(mockFactory.Object);
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
    }
}
