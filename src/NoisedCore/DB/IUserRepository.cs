﻿using Noised.Core.UserManagement;

namespace Noised.Core.DB
{
    /// <summary>
    /// Interface of a UserRepository
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Creates a new User in the current IUnitOfWork implementaion
        /// </summary>
        /// <param name="user">User to create</param>
        void CreateUser(User user);

        /// <summary>
        /// Updates an existing User in the current IUnitOfWork implementaion
        /// </summary>
        /// <param name="user">User to update</param>
        void UpdateUser(User user);

        /// <summary>
        /// Deletes a User in the current IUnitOfWork implementation
        /// </summary>
        /// <param name="user">User to delete</param>
        void DeleteUser(User user);

        /// <summary>
        /// Gets a User by its Name
        /// </summary>
        /// <param name="username">The name of the user to get</param>
        /// <returns>The User</returns>
        User GetUser(string username);
    }
}
