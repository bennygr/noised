using System;

namespace Noised.Core.Media
{
    public class PlaylistAlreadyExistsException : Exception
    {
        public PlaylistAlreadyExistsException(string message)
            : base(message)
        { }

        public PlaylistAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
