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
    }
}