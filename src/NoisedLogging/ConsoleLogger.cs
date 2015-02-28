using System;

namespace Noised.Logging
{
    /// <summary>
    ///		A simple Logger to write messages to stdout
    /// </summary>
    public class ConsoleLogger : ILogging
    {
        private readonly string header = string.Empty;

		/// <summary>
		///		Constructor
		/// </summary>
        public ConsoleLogger()
            : this(string.Empty) { }

		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name='header'>A header which is written before every logged line</param>
        public ConsoleLogger(string header)
        {
            this.header = header;
        }

        #region ILogging Member

        public void LogMessage(LogLevel level, string message)
        {
            string fullHeader;
            if (header == string.Empty)
            {
                fullHeader = "[" + level + "]: ";
            }
            else
            {
                fullHeader = header;
            }
            Console.WriteLine(DateTime.Now.ToShortTimeString() + 
							  ":" + 
							  DateTime.Now.Second.ToString("00") + 
							  " " + 
							  fullHeader +
                              message);
        }

        #endregion
    }
}
