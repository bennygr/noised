using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Noised.Logging;

namespace Noised.Core.Config.Filesystem
{
    /// <summary>
    ///		Loads configuration data from a given directory from the filesystem
    /// </summary>
    public class FilesystemConfigurationLoader : IConfigurationLoader
    {
        private string configPath;
        private ILogging logging;

        /// <summary>
        ///		Constructor
        /// </summary>
        public FilesystemConfigurationLoader(ILogging logging)
        {
            this.logging = logging;
            this.configPath =
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                Path.DirectorySeparatorChar +
                "etc";
        }

        /// <summary>
        ///		Internal method to load the configuration files 
        /// </summary>
        private IEnumerable<ConfigurationData> LoadAllSources()
        {
            var ret = new List<ConfigurationData>();
            logging.Debug(String.Format("Loading *.nconfig files from directory {0}", configPath));
            if (Directory.Exists(configPath))
            {
                //Loading all the nconfig files in the config folder
                var configFiles = Directory.GetFiles(configPath, "*.nconfig");
                if (configFiles.Length > 0)
                {
                    foreach (var configFile in configFiles)
                    {
                        ret.Add(new ConfigurationData
                                {
                                    Name = configFile,
                                    Data = System.IO.File.ReadAllText(configFile)
                                });
                    }
                }
                else
                {
                    logging.Warning(String.Format(
                            "Could not find any *.config in directory {0}.",
                            configPath));
                }
            }
            else
            {
                logging.Error(String.Format(
                        "Could not load *.config files from directory {0}. Directory does not exist.",
                        configPath));
            }
            return ret;
        }

        #region IConfigSource implementation

        public IEnumerable<ConfigurationData> LoadData()
        {
            return LoadAllSources();
        }

        #endregion
    };
}
