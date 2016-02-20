using Noised.Core.DB;

namespace Noised.Core.UserManagement
{
    /// <summary>
    /// Interface for a UserManager
    /// </summary>
    public interface IUserManager
    {
        /// <summary>
        /// Sets the IDbFactory
        /// </summary>
        IDbFactory DbFactory { set; }

        /// <summary>
        /// Authenticates a User
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>true if the User is successfully authenticated</returns>
        bool Authenticate(string username, string password);
    }
}