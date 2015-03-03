namespace Noised.Logging
{
    /// <summary>
    ///		A simple interface representing a logger
    /// </summary>
    /// <remarks>
    ///		WARNING: If implementing this interface to build your own logger
    /// 	your implementation of LogMessage() must not do any operations
    /// 	which takes a long time, because the LoggerFaccade 
    /// 	is locked during a log-operation due the multithreading support      
    /// </remarks>
    public interface ILogger
    {
        /// <summary>
        /// Logging 
        /// </summary>
        /// <param name="level">the message's level</param>
        /// <param name="message">the message to log</param>
        void LogMessage(LogLevel level,string message);
    }
}
