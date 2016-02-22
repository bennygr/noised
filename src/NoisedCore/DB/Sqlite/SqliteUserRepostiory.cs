using System;
using System.Data;
using Mono.Data.Sqlite;
using Noised.Core.UserManagement;

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
            if (connection == null)
                throw new ArgumentNullException("connection");

            this.connection = connection;
        }

        /// <summary>
        /// Creates a User
        /// </summary>
        /// <param name="user">User to create</param>
        public void CreateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            using (SqliteCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = UserSql.CreateUser;
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add(new SqliteParameter("@Username", user.Name));
                cmd.Parameters.Add(new SqliteParameter("@Password", user.Password));

                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Deletes a User
        /// </summary>
        /// <param name="user">User to delete</param>
        public void DeleteUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            using (SqliteCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = UserSql.DeleteUser;
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add(new SqliteParameter("@Username", user.Name));

                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Gets a User by its Name
        /// </summary>
        /// <param name="username">The name of the user to get</param>
        /// <returns>The User</returns>
        public User GetUser(string username)
        {
            if (String.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException("username");

            DataTable userTable = new DataTable();

            using (SqliteCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = UserSql.GetUser;
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add(new SqliteParameter("@Username", username));

                userTable.Load(cmd.ExecuteReader());
            }

            if (userTable.Rows.Count > 1)
                throw new UserRepositoryException("More than one user was foung for username \"" + username + "\"");

            if (userTable.Rows.Count < 1)
                return null;

            return new User(userTable.Rows[0]["Username"].ToString())
            {
                Password = userTable.Rows[0]["Password"].ToString()
            };
        }

        /// <summary>
        /// Updates a User
        /// </summary>
        /// <param name="user">User to update</param>
        public void UpdateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            DeleteUser(user);
            CreateUser(user);
        }
    }
}
