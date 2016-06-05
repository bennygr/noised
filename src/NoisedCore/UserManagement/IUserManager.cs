using Noised.Core.DB;

namespace Noised.Core.UserManagement
{
    /// <summary>
    /// Interface for a UserManager
    /// </summary>
    public interface IUserManager
    {
        /// <summary>
        /// Authenticates an User
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>true if the User is successfully authenticated</returns>
        bool Authenticate(string username, string password);

        /// <summary>
        /// Creates an new User
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Pswword</param>
        /// <returns>The new created user</returns>
        User CreateUser(string username, string password);

        /// <summary>
        /// Deletes an User
        /// </summary>
        /// <param name="user">User to delete</param>
        void DeleteUser(User user);

        /// <summary>
        /// Updates an User
        /// </summary>
        /// <param name="user">User to Update</param>
        void UpdateUser(User user);

        /// <summary>
        /// Gets a User by its name
        /// </summary>
        /// <param name="username">Name of the user</param>
        /// <returns>The User with the given name</returns>
        User GetUser(string username);
    }
}
