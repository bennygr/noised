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
        Info,
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
                lock (SyncObject)
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

        #region ILogging

        public void AddLogger(ILogger logger)
        {
            Loggers.Add(logger);
        }
		
        public void Debug(string message)
        {
            LogInternal(LogLevel.Debug, message);
        }
		
        public void Warning(string message)
        {
            LogInternal(LogLevel.Warning, message);
        }
		
        public void Info(string message)
        {
            LogInternal(LogLevel.Info, message);
        }
		
        public void Error(string message)
        {
            LogInternal(LogLevel.Error, message);
        }

        #endregion
    }
}
