using System;
using System.Collections.Generic;
using System.IO;
using Noised.Logging;

namespace Noised.Core.Config
{
    /// <summary>
    ///		Default implementation of IConfig for property handling
    /// </summary>
    public class Config : IConfig
    {
        #region Fields

        private readonly ILogging logging;
        private readonly Dictionary<string, string> properties;
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
            properties = new Dictionary<string, string>();
            this.logging = logging;
        }

        #endregion

        #region Methods

        /// <summary>
        ///		Internal method to read and parse the properties from the given loader
        /// </summary>
        /// <param name="loader">the loader to read data from</param>
        private void LoadProperties(IConfigurationLoader loader)
        {
            var configurationData = loader.LoadData();
            foreach (var data in configurationData)
            {
                string rawStringData = data.Data;
                if (rawStringData != null)
                {
                    using (var sourceReader = new StringReader(rawStringData))
                    {
                        string line;
                        int lineCounter = 0;
                        do
                        {
                            lineCounter++;
                            line = sourceReader.ReadLine();
                            //Filter comments
                            if (line != null &&
                                line.Trim() != String.Empty &&
                                !line.StartsWith(CharComment.ToString(), StringComparison.Ordinal))
                            {
                                var splits = line.Split(CharAssign);
                                if (splits.Length == 2)
                                {
                                    var property = splits[0].Trim();
                                    var value = splits[1].Trim();
                                    properties[property] = value;
                                    logging.Debug(String.Format("{0} property loaded ", property));
                                }
                                else
                                {
                                    logging.Error(
                                            String.Format("Failed to load value {0} from {1} at line {2}. Could not split line.",
                                                line,
                                                data.Name,
                                                lineCounter
                                                ));
                                }
                            }
                        } while (line != null);
                    }
                }
            }
            logging.Debug(String.Format("{0} properties loaded from configuration",
                                        properties.Count));

        }

        #endregion


        #region IConfig implementation

        public string GetProperty(string property, string defaultvalue = null)
        {
            return (properties.ContainsKey(property) ? properties[property] : defaultvalue);
        }

        public void Load(IConfigurationLoader loader)
        {
            properties.Clear();
            LoadProperties(loader);
        }

        public int Count
        {
            get { return properties.Count; }
        }

        #endregion
    };

}
