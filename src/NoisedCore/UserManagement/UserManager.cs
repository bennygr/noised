using System;
using Noised.Core.Crypto;
using Noised.Core.DB;
using PasswordSecurity;

namespace Noised.Core.UserManagement
{
    /// <summary>
    /// Manages all the Users
    /// </summary>
    public class UserManager : IUserManager
    {
        private readonly IDbFactory _dbFactory;
        private readonly IPasswordManager _passwordManager;

        /// <summary>
        /// Manages all the Users
        /// </summary>
        /// <param name="dbFactory"></param>
        /// <param name="passwordManager"></param>
        public UserManager(IDbFactory dbFactory, IPasswordManager passwordManager)
        {
            _dbFactory = dbFactory;
            _passwordManager = passwordManager;
        }

        #region IUserManager
        
        public bool Authenticate(string username, string password)
        {
            User user;
            using (IUnitOfWork iuw = _dbFactory.GetUnitOfWork())
                user = iuw.UserRepository.GetUser(username);
        
            if (user != null && _passwordManager.VerifyPassword(password, user.PasswordHash))
                return true;
            return false;
        }
        
        public User CreateUser(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(username));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(password));

            string hashedPw = _passwordManager.CreateHash(password);
        
            using (IUnitOfWork iuw = _dbFactory.GetUnitOfWork())
            {
                var user = new User(username) { PasswordHash = hashedPw };
                iuw.UserRepository.CreateUser(user);
                iuw.SaveChanges();
                return user;
            }
        }
        
        public void DeleteUser(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            using (IUnitOfWork iuw = _dbFactory.GetUnitOfWork())
            {
                iuw.UserRepository.DeleteUser(user);
                iuw.SaveChanges();
            }
        }

        public void UpdateUser(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            using (IUnitOfWork iuw = _dbFactory.GetUnitOfWork())
            {
                iuw.UserRepository.UpdateUser(user);
                iuw.SaveChanges();
            }
        }

        public User GetUser(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(username));

            using (IUnitOfWork iuw = _dbFactory.GetUnitOfWork())
                return iuw.UserRepository.GetUser(username);
        }

        #endregion
    }
}
