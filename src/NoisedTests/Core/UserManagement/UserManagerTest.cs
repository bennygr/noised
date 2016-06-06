using System;
using Moq;
using Noised.Core.Crypto;
using Noised.Core.DB;
using Noised.Core.UserManagement;
using NUnit.Framework;

namespace NoisedTests.Core.UserManagement
{
    [TestFixture]
    public class UserManagerTest
    {
        [Test]
        public void UserManager_Constructor_AllParameters_CanCreateInstace()
        {
            var dbFacMock = new Mock<IDbFactory>();
            var passManMock = new Mock<IPasswordManager>();

            new UserManager(dbFacMock.Object, passManMock.Object);
        }

        [Test]
        public void UserManager_Constructor_DBFactoryNull_ShouldThrowArgumentNullException()
        {
            var passManMock = new Mock<IPasswordManager>();

            try
            {
                new UserManager(null, passManMock.Object);

                Assert.Fail("Should have thrown exception");
            }
            catch (ArgumentNullException e)
            {
                StringAssert.AreEqualIgnoringCase("dbFactory", e.ParamName);
            }
        }

        [Test]
        public void UserManager_Constructor_PasswordManagerNull_ShouldThrowArgumentNullException()
        {
            var dbFacMock = new Mock<IDbFactory>();

            try
            {
                new UserManager(dbFacMock.Object, null);

                Assert.Fail("Should have thrown exception");
            }
            catch (ArgumentNullException e)
            {
                StringAssert.AreEqualIgnoringCase("passwordManager", e.ParamName);
            }
        }

        [Test]
        public void UserManager_Authenticate_ValidUser_ReturnsTrue()
        {
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(x => x.GetUser(It.IsAny<string>())).Returns(new User("username") { PasswordHash = "hash" });

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x => x.UserRepository).Returns(userRepoMock.Object);

            var dbFacMock = new Mock<IDbFactory>();
            dbFacMock.Setup(x => x.GetUnitOfWork()).Returns(uowMock.Object);

            var passManMock = new Mock<IPasswordManager>();
            passManMock.Setup(x => x.VerifyPassword("password", "hash")).Returns(true);

            var uman = new UserManager(dbFacMock.Object, passManMock.Object);
            var result = uman.Authenticate("username", "password");

            Assert.IsTrue(result);
        }

        [Test]
        public void UserManager_Authenticate_InValidUser_ReturnsFalse()
        {
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(x => x.GetUser(It.IsAny<string>())).Returns(() => null);

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x => x.UserRepository).Returns(userRepoMock.Object);

            var dbFacMock = new Mock<IDbFactory>();
            dbFacMock.Setup(x => x.GetUnitOfWork()).Returns(uowMock.Object);

            var passManMock = new Mock<IPasswordManager>();

            var uman = new UserManager(dbFacMock.Object, passManMock.Object);
            var result = uman.Authenticate("username", "password");

            Assert.IsFalse(result);
        }

        [Test]
        public void UserManager_Authenticate_InValidPassword_ReturnsFalse()
        {
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(x => x.GetUser(It.IsAny<string>())).Returns(new User("username") { PasswordHash = "hash" });

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x => x.UserRepository).Returns(userRepoMock.Object);

            var dbFacMock = new Mock<IDbFactory>();
            dbFacMock.Setup(x => x.GetUnitOfWork()).Returns(uowMock.Object);

            var passManMock = new Mock<IPasswordManager>();
            passManMock.Setup(x => x.VerifyPassword("password", "hash")).Returns(false);

            var uman = new UserManager(dbFacMock.Object, passManMock.Object);
            var result = uman.Authenticate("username", "password");

            Assert.IsFalse(result);
        }

        [Test]
        public void UserManager_Authenticate_NullPassword_ThrowsArgumentNullException()
        {
            try
            {
                var dbFacMock = new Mock<IDbFactory>();
                var passManMock = new Mock<IPasswordManager>();

                var userMan = new UserManager(dbFacMock.Object, passManMock.Object);
                userMan.Authenticate("username", null);
            }
            catch (ArgumentException e)
            {
                StringAssert.AreEqualIgnoringCase("password", e.ParamName);
            }
        }

        [Test]
        public void UserManager_Authenticate_EmptyPassword_ThrowsArgumentNullException()
        {
            try
            {
                var dbFacMock = new Mock<IDbFactory>();
                var passManMock = new Mock<IPasswordManager>();

                var userMan = new UserManager(dbFacMock.Object, passManMock.Object);
                userMan.Authenticate("username", String.Empty);
            }
            catch (ArgumentException e)
            {
                StringAssert.AreEqualIgnoringCase("password", e.ParamName);
            }
        }

        [Test]
        public void UserManager_Authenticate_NullUsername_ThrowsArgumentNullException()
        {
            try
            {
                var dbFacMock = new Mock<IDbFactory>();
                var passManMock = new Mock<IPasswordManager>();

                var userMan = new UserManager(dbFacMock.Object, passManMock.Object);
                userMan.Authenticate(null, "password");
            }
            catch (ArgumentException e)
            {
                StringAssert.AreEqualIgnoringCase("username", e.ParamName);
            }
        }

        [Test]
        public void UserManager_Authenticate_EmptyUsername_ThrowsArgumentNullException()
        {
            try
            {
                var dbFacMock = new Mock<IDbFactory>();
                var passManMock = new Mock<IPasswordManager>();

                var userMan = new UserManager(dbFacMock.Object, passManMock.Object);
                userMan.Authenticate(String.Empty, "password");
            }
            catch (ArgumentException e)
            {
                StringAssert.AreEqualIgnoringCase("username", e.ParamName);
            }
        }

        [Test]
        public void UserManager_CreateUser_ValidParametersProvided_CanCreateUser()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void UserManager_CreateUser_NoUsernameProvided_ThrowArgumentException()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void UserManager_CreateUser_NoPasswordProvided_ThrowArgumentException()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void UserManager_DeleteUser_ValidParametersProvided_CanDeleteUser()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void UserManager_DeleteUser_NoUserProvided_ThrowArgumentNullException()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void UserManager_UpdateUser_ValidParametersProvided_CanUpdateUser()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void UserManager_UpdateUser_NoUserProvided_ThrowArgumentNullException()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void UserManager_GetUser_ValidParametersProvided_CanGetUser()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void UserManager_GetUser_NoUsernameProvided_ThrowArgumentException()
        {
            Assert.Inconclusive();
        }
    }
}