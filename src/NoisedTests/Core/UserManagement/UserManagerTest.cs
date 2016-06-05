using NUnit.Framework;

namespace NoisedTests.Core.UserManagement
{
    [TestFixture]
    public class UserManagerTest
    {
        [Test]
        public void UserManager_Authenticate_InValidUser_ReturnsFalse()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void UserManager_Authenticate_NoPassword_ThrowsArgumentNullException()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void UserManager_Authenticate_NoUsername_ShouldThrowArgumentNullExceptio()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void UserManager_Authenticate_NoUsername_ThrowsArgumentNullException()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void UserManager_Authenticate_ValidUser_ReturnsTrue()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void UserManager_Constructor_AllParameters_CanCreateInstace()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void UserManager_Constructor_DBFactoryNull_ShouldThrowArgumentNullException()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void UserManager_Constructor_PasswordManagerNull_ShouldThrowArgumentNullException()
        {
            Assert.Inconclusive();
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