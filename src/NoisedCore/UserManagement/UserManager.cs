using System;
using Noised.Core.Crypto;
using Noised.Core.DB;

namespace Noised.Core.UserManagement
{
    /// <summary>
    /// Manages all the Users
    /// </summary>
    public class UserManager : IUserManager
    {
        private readonly IDbFactory dbFactory;
        private readonly IPasswordManager passwordManager;

        /// <summary>
        /// Manages all the Users
        /// </summary>
        /// <param name="dbFactory"></param>
        /// <param name="passwordManager"></param>
        public UserManager(IDbFactory dbFactory, IPasswordManager passwordManager)
        {
            if (dbFactory == null)
                throw new ArgumentNullException("dbFactory");
            if (passwordManager == null)
                throw new ArgumentNullException("passwordManager");

            this.dbFactory = dbFactory;
            this.passwordManager = passwordManager;
        }

        #region IUserManager

        public bool Authenticate(string username, string password)
        {
            if (String.IsNullOrWhiteSpace(username))
                throw new ArgumentException("username cannot be null or empty", "username");
            if (String.IsNullOrWhiteSpace(password))
                throw new ArgumentException("password cannot be null or empty", "password");

            User user;
            using (IUnitOfWork iuw = dbFactory.GetUnitOfWork())
                user = iuw.UserRepository.GetUser(username);

            if (user != null && passwordManager.VerifyPassword(password, user.PasswordHash))
                return true;
            return false;
        }

        public User CreateUser(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Value cannot be null or whitespace.", "username");
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be null or whitespace.", "password");

            string hashedPw = passwordManager.CreateHash(password);

            var user = new User(username) { PasswordHash = hashedPw };

            using (IUnitOfWork iuw = dbFactory.GetUnitOfWork())
            {
                iuw.UserRepository.CreateUser(user);
                iuw.SaveChanges();
            }

            return user;
        }

        public void DeleteUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user", "");

            using (IUnitOfWork iuw = dbFactory.GetUnitOfWork())
            {
                iuw.UserRepository.DeleteUser(user);
                iuw.SaveChanges();
            }
        }

        public void UpdateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            using (IUnitOfWork iuw = dbFactory.GetUnitOfWork())
            {
                iuw.UserRepository.UpdateUser(user);
                iuw.SaveChanges();
            }
        }

        public User GetUser(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Value cannot be null or whitespace.", "username");

            using (IUnitOfWork iuw = dbFactory.GetUnitOfWork())
                return iuw.UserRepository.GetUser(username);
        }

        #endregion
    }
}
