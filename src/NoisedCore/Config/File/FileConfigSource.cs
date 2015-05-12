using System;
using System.IO;
using System.Reflection;
using System.Text;
using Noised.Logging;
using Noised.Core.Config;

namespace Noised.Core.Config
{
    class FileConfigurationSource : IConfigSource
    {
        private string configPath;
        private ILogging logging;

        /// <summary>
        ///		Constructor
        /// </summary>
        FileConfigurationSource(ILogging logging)
        {
            this.logging = logging;
            this.configPath = 
				Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
				Path.DirectorySeparatorChar +
            	"etc";
        }

        private string LoadAllSources()
        {
            logging.Debug(String.Format("Loading *.nconfig files from directory {0}", configPath));
            if (Directory.Exists(configPath))
            {
                //Loading all the nconfig files in the config folder
                var source = new StringBuilder();
                var configFiles = Directory.GetFiles(configPath, "*.nconfig");
                foreach (var configFile in configFiles)
                {
                    source.Append(System.IO.File.ReadAllText(configFile));
                }
                return source.ToString();
            }

            logging.Error(String.Format(
                    "Could not load *.config files from directory {0}. Directory does not exist.",
                    configPath));
            return null;
        }


        #region IConfigSource implementation

        public string GetSourceData()
        {
            return LoadAllSources();
        }

        #endregion
    };
}
