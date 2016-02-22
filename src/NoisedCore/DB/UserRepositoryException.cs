using System;

namespace Noised.Core.DB
{
    public class UserRepositoryException : Exception
    {
        public UserRepositoryException(string message)
            : base(message)
        { }
    }
}
