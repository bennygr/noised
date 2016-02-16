using System;

namespace Noised.Core.UserManagement
{
    public class UserManagerException : Exception
    {
        public UserManagerException(string message) : base(message)
        { }
    }
}
