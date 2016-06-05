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

        /// <summary>
        /// Manages all the Users
        /// </summary>
        /// <param name="dbFactory"></param>
        public UserManager(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        #region IUserManager
        
        public bool Authenticate(string username, string password)
        {
            User user;
            using (IUnitOfWork iuw = _dbFactory.GetUnitOfWork())
                user = iuw.UserRepository.GetUser(username);
        
            if (user != null && PasswordStorage.VerifyPassword(password, user.PasswordHash))
                return true;
            return false;
        }
        
        public User CreateUser(string username, string password)
        {
            string hashedPw = PasswordStorage.CreateHash(password);
        
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
            using (IUnitOfWork iuw = _dbFactory.GetUnitOfWork())
            {
                iuw.UserRepository.DeleteUser(user);
                iuw.SaveChanges();
            }
        }
        
        public void UpdateUser(User user)
        {
            using (IUnitOfWork iuw = _dbFactory.GetUnitOfWork())
            {
                iuw.UserRepository.UpdateUser(user);
                iuw.SaveChanges();
            }
        }
        
        public User GetUser(string username)
        {
            using (IUnitOfWork iuw = _dbFactory.GetUnitOfWork())
                return iuw.UserRepository.GetUser(username);
        }
        
        #endregion
    }
}
