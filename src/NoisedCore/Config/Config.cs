using System;
using System.Collections.Generic;
using System.IO;
using Noised.Logging;

namespace Noised.Core.Config
{
    /// <summary>
    ///		Default implementation for property handling
    /// </summary>
    public class Config : IConfig
    {
        #region Fields

        private readonly ILogging logging;
        private readonly Dictionary<string,string> properties;
        private const char CharComment = '#';
        private const char CharAssign = '=';

        #endregion

        #region Constructor

        /// <summary>
        ///		Constructor
        /// </summary>
        /// <param name="logging">The logging object</param>
        public Config(ILogging logging)
        {
            this.properties = new Dictionary<string,string>();
            this.logging = logging;
        }

        #endregion

        #region Methods

		/// <summary>
		///		Internal method to read and parse the properties from the given source
		/// </summary>
		/// <param name="source">The configuration source to read data from</param>
        private void LoadProperties(IConfigSource source)
        {
            using (var sourceReader = new StringReader(source.GetSourceData()))
            {
				string line;
				do
                {
					line = sourceReader.ReadLine();
					//Filter comments
                    if (line != null && 
						!line.StartsWith(CharComment.ToString(), StringComparison.Ordinal)) 
                    {
                        var splits = line.Split(CharAssign);
                        if (splits.Length == 2)
                        {
                            var property = splits[0].Trim();
                            var value = splits[1].Trim();
                            properties[property] = value;
                            logging.Debug(String.Format("Added property {0}", property));
                        }
                        else
                        {
                            logging.Error(
                                String.Format("Failed to load value {0}. Could not split line.",
                                    line));
                        }
                    }
                } while (line != null);
            }
        }

        #endregion


        #region IConfig implementation

        public string GetProperty(string property, string defaultvalue = null)
        {
            return (properties.ContainsKey(property) ? properties[property] : defaultvalue);
        }

        public void Load(IConfigSource source)
        {
            properties.Clear();
            LoadProperties(source);
        }

		public int Count()
		{
			return properties.Count;
		}

        #endregion
    };

}
