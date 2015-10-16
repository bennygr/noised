using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Noised.Logging;
using Noised.Core.IOC;
namespace Noised.Core.Plugins
{
    /// <summary>
    ///		Default plugin installer
    /// </summary>
    public class PluginInstaller : IPluginInstaller
    {
        #region IPluginInstaller implementation

        public void InstallAll(string path)
        {
			var files = Directory.GetFiles(path).Where(
							file => file.EndsWith(".npluginz", StringComparison.Ordinal) );

			//Extract all nplugins file to the tmp directory and process them from there
            foreach (var file in files)
            {
				IocContainer.Get<ILogging>().Debug(String.Format("Found new plugin {0}...",file));
				Install(file);
            }
        }

		public void Install(string npluginzFilePath)
		{
			//Create a tmp if not existing yet directory
			string tmpPath = "." + Path.DirectorySeparatorChar + "tmp";
			if(!Directory.Exists(tmpPath))
			{
				Directory.CreateDirectory(tmpPath);
			}

			//Creating a tmp dir 
			string tmpPluginPath = tmpPath + Path.DirectorySeparatorChar + Guid.NewGuid();
			IocContainer.Get<ILogging>().Debug(String.Format("Installing new plugin {0}...",npluginzFilePath));
			//..and extract the plugin content to the dir
			System.IO.Compression.ZipFile.ExtractToDirectory(npluginzFilePath,tmpPluginPath);
			IocContainer.Get<ILogging>().Debug(
					String.Format("Plugin {0} has been extracted to {1}",npluginzFilePath,tmpPluginPath));

			//Reading the plugin's meta data
			string metaFile = tmpPluginPath + Path.DirectorySeparatorChar + "plugin.nplugininfo";
			if(!File.Exists(metaFile))
			{
				throw new FileNotFoundException("Plugin does not contain a plugin.nplugininfo file");
			}
			string metaContent = File.ReadAllText(metaFile);
			PluginRegistrationData pluginData = 
				JsonConvert.DeserializeObject<PluginRegistrationData>(metaContent);

			//TODO:copy the plugin Content to the noised dir 
			
			//TODO: Create meta data for each file

			//TODO:Write the meta data to the DB

			//TODO:cleanup
			//Directory.Delete(tmpPluginPath);
			File.Delete(npluginzFilePath);
		}

        #endregion
    };
}
