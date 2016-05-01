using Noised.Core.DB;
using PasswordSecurity;

namespace Noised.Core.UserManagement
{
    /// <summary>
    /// Manages all the Users
    /// </summary>
    public class UserManager : IUserManager
    {
        private IDbFactory dbFactory;

        /// <summary>
        /// Manages all the Users
        /// </summary>
        /// <param name="dbFactory"></param>
        public UserManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        #region IUserManager
        
        public IDbFactory DbFactory
        {
            private get
            {
                if (dbFactory == null)
                    throw new UserManagerException("You need to set the DbFactory Property first!");
        
                return dbFactory;
            }
            set
            {
                dbFactory = value;
            }
        }
        
        public bool Authenticate(string username, string password)
        {
            User user;
            using (IUnitOfWork iuw = dbFactory.GetUnitOfWork())
                user = iuw.UserRepository.GetUser(username);
        
            if (user != null && PasswordStorage.VerifyPassword(password, user.PasswordHash))
                return true;
            return false;
        }
        
        public User CreateUser(string username, string password)
        {
            string hashedPw = PasswordStorage.CreateHash(password);
        
            using (IUnitOfWork iuw = dbFactory.GetUnitOfWork())
            {
                var user = new User(username) { PasswordHash = hashedPw };
                iuw.UserRepository.CreateUser(user);
                iuw.SaveChanges();
                return user;
            }
        }
        
        public void DeleteUser(User user)
        {
            using (IUnitOfWork iuw = dbFactory.GetUnitOfWork())
            {
                iuw.UserRepository.DeleteUser(user);
                iuw.SaveChanges();
            }
        }
        
        public void UpdateUser(User user)
        {
            using (IUnitOfWork iuw = dbFactory.GetUnitOfWork())
            {
                iuw.UserRepository.UpdateUser(user);
                iuw.SaveChanges();
            }
        }
        
        public User GetUser(string username)
        {
            using (IUnitOfWork iuw = dbFactory.GetUnitOfWork())
                return iuw.UserRepository.GetUser(username);
        }
        
        #endregion
    }
}
