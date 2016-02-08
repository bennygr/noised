using System;

namespace Noised.Core.Media
{
    public class MediaItemNotFoundException : Exception
    {
        public MediaItemNotFoundException(string message)
            : base(message)
        { }

        public MediaItemNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
