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

        /// <summary>
        /// Authenticate a User
        /// </summary>
        /// <param name="username">Login of the User</param>
        /// <param name="password">Password(hash) of the User</param>
        /// <returns></returns>
        public bool Authenticate(string username, string password)
        {
            User user;
            using (IUnitOfWork iuw = dbFactory.GetUnitOfWork())
                user = iuw.UserRepository.GetUser(username);

            if (user != null && PasswordStorage.VerifyPassword(password, user.Password))
                return true;
            return false;
        }

        /// <summary>
        /// Sets a Factory for creating instances of IUnitOfWork to access the Database
        /// </summary>
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

        /// <summary>
        /// Creates a new User
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        public void CreateUser(string username, string password)
        {
            string hashedPw = PasswordStorage.CreateHash(password);

            using (IUnitOfWork iuw = dbFactory.GetUnitOfWork())
            {
                iuw.UserRepository.CreateUser(new User(username) { Password = hashedPw });
                iuw.SaveChanges();
            }
        }
    }
}
