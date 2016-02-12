using System;

namespace Noised.Core.Media
{
    /// <summary>
    /// Exception should be thrown when a MediaItem is not found in the given sources
    /// </summary>
    public class MediaItemNotFoundException : Exception
    {
        /// <summary>
        /// Exception should be thrown when a MediaItem is not found in the given sources
        /// </summary>
        /// <param name="message">Message of the Exception</param>
        public MediaItemNotFoundException(string message)
            : base(message)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message">Message of the Exception</param>
        /// <param name="innerException">Inner Exception</param>
        public MediaItemNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
