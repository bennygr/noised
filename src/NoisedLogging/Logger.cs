using System;
using System.Collections.Generic;

namespace Noised.Logging
{
    /// <summary>
    /// Represents a LogLevel
    /// </summary>
    public enum LogLevel
    {
        Debug,
        Warning,
        Error
    }

    /// <summary>
    ///		A faccade for writing messages to a set of loggers
    /// </summary>
    public class Logger : ILogging
    {
        private LogLevel logLevel;
        private readonly Object SyncObject = new object();
        private readonly List<ILogger> Loggers = new List<ILogger>();

        /// <summary>
        ///		The current Loglevel
        /// </summary>
        private LogLevel LogLevel
        {
            get
            {
                lock(SyncObject)
                {
                    return logLevel;
                }
            }
            set
            {
                lock (SyncObject)
                {
                    logLevel = value;
                }
            }
        }

        /// <summary>
        ///		Internal Log-Method
        /// </summary>
        /// <param name="level">The message's loglevel</param>
        /// <param name="message">The message to log</param>
        private void LogInternal(LogLevel level, string message)
        {
            lock (SyncObject)
            {
                if (level >= logLevel)
                {
                    foreach (ILogger logging in Loggers)
                    {
                        logging.LogMessage(level, message);
                    }
                }
            }
        }

        /// <summary>
        ///		Adds a logger to the logging system
        /// </summary>
        /// <param name="logger">the logger to add</param>
        public void AddLogger(ILogger logger)
        {
            Loggers.Add(logger);
        }
                
        /// <summary>
        ///		Writes a debug message to all registered loggers
        /// </summary>
        /// <param name="message">The message</param>
        public void Debug(string message)
        {
            LogInternal(LogLevel.Debug, message);
        }

        /// <summary>
        ///		Writes a warning message to all registered logger
        /// </summary>
        /// <param name="message">The message</param>
        public void Warning (string message)
        {
            LogInternal(LogLevel.Warning, message);
        }

        /// <summary>
        ///		Writes an error message to all registered logger
        /// </summary>
        /// <param name="message">The message</param>        
        public void Error(string message)
        {
            LogInternal(LogLevel.Error, message);
        }
    }
}
