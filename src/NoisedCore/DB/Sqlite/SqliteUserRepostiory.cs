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
        /// Gets all Users
        /// </summary>
        public IList<User> Users
        {
            get
            {
                DataTable userTable = new DataTable();

                using(SqliteCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = UserSql.SelectAllusers;
                    cmd.CommandType = CommandType.Text;

                    userTable.Load(cmd.ExecuteReader());
                }

                List<User> users = new List<User>();
                foreach(DataRow row in userTable.Rows)
                    users.Add(new User(row["Username"].ToString(), row["Password"].ToString()));

                return users;
            }
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
        /// Updates a User
        /// </summary>
        /// <param name="user">User to update</param>
        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
