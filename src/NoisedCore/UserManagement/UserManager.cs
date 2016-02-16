using System.Collections.Generic;
using Noised.Core.DB;
using System.Linq;

namespace Noised.Core.UserManagement
{
    /// <summary>
    /// Manages all the Users
    /// </summary>
    public class UserManager : IUserManager
    {
        private IDbFactory dbFactory;
        private List<User> users;

        /// <summary>
        /// Manages all the Users
        /// </summary>
        /// <param name="dbFactory"></param>
        public UserManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
            users = new List<User>();
        }

        /// <summary>
        /// Authenticate a User
        /// </summary>
        /// <param name="username">Login of the User</param>
        /// <param name="password">Password(hash) of the User</param>
        /// <returns></returns>
        public bool Authenticate(string username, string password)
        {
            if(username == "benny" && password == "test")
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
                if(dbFactory == null)
                    throw new UserManagerException("You need to set the DbFactory Property first!");

                return dbFactory;
            }
            set
            {
                dbFactory = value;
            }
        }

        /// <summary>
        /// Loads all Users from the Database
        /// </summary>
        public void LoadUsers()
        {
            lock (users)
            {
                using(IUnitOfWork uow = DbFactory.GetUnitOfWork())
                    users = uow.UserRepository.Users.ToList();
            }
        }
    }
}
