using System;
using System.Collections.Generic;
using Noised.Core.UserManagement;
using Mono.Data.Sqlite;
using System.Data;

namespace Noised.Core.DB.Sqlite
{
    /// <summary>
    /// UserRepository for Sqlite
    /// </summary>
    internal class SqliteUserRepostiory : IUserRepository
    {
        private readonly SqliteConnection connection;

        public SqliteUserRepostiory(SqliteConnection connection)
        {
            if(connection == null)
                throw new ArgumentNullException("connection");

            this.connection = connection;
        }

        /// <summary>
        /// Creates a User
        /// </summary>
        /// <param name="user">User to create</param>
        public void CreateUser(User user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes a User
        /// </summary>
        /// <param name="user">User to delete</param>
        public void DeleteUser(User user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a User by its Name
        /// </summary>
        /// <param name="username">The name of the user to get</param>
        /// <returns>The User</returns>
        public User GetUser(string username)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates a User
        /// </summary>
        /// <param name="user">User to update</param>
        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
