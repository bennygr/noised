using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Noised.Logging;
using Noised.Core.DB;
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

			//Checking if the plugin is already installed
			using(IUnitOfWork unitOfWork = IocContainer.Get<IUnitOfWork>())
			{
				var existingPlugin = unitOfWork.PluginRepository.GetByGuid(pluginData.Guid);
                if (existingPlugin == null || existingPlugin.GetVersion() < pluginData.GetVersion())
                {
					//Install the plugin
					//Installing plugin configuration  
					var etcPath = new DirectoryInfo(tmpPluginPath + Path.DirectorySeparatorChar + "etc");
					string noisedEtcPath = 
						Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
						Path.DirectorySeparatorChar +
						"etc";
					var installedFiles = new List<FileInfo>();
					foreach (var etcFile in etcPath.GetFiles())
                    {
						//Copy config file
						//TODO: We have the configuration abstracted by using IConfigurationLoader
						//but when handling with plugins we asume a FileSystem configuration
						//this should be abstracted as well somehow
						var installedFile = new FileInfo(noisedEtcPath + Path.DirectorySeparatorChar + etcFile.Name);
						File.Copy(etcFile.FullName, installedFile.FullName);
						installedFiles.Add(installedFile);
                    }
					//Installing plugin files  
					var pluginsPath  = new DirectoryInfo(tmpPluginPath + Path.DirectorySeparatorChar + "plugins");
					string noisedPluginsPath = 
						Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
						Path.DirectorySeparatorChar +
						"plugins";
					foreach (var pluginFile in pluginsPath.GetFiles())
                    {
						//Copy plugin file
						var installedFile = new FileInfo(noisedPluginsPath + Path.DirectorySeparatorChar + pluginFile.Name);
						File.Copy(pluginFile.FullName, installedFile.FullName);
						installedFiles.Add(installedFile);
                    }
					
					//Registering the plugin into the DB
					unitOfWork.PluginRepository.RegisterPlugin(pluginData,installedFiles);
                    IocContainer.Get<ILogging>().Debug(
							String.Format("Plugin {0} has been installed (Version {1})",
										   pluginData.Name,
										   pluginData.Version));
                }
                else
                {
                    IocContainer.Get<ILogging>().Debug(
							String.Format("Plugin {0} is already installed with version {1} or newer",
										   pluginData.Name,
										   pluginData.Version));
                }
				unitOfWork.SaveChanges();
			}

			//Clean up
			Directory.Delete(tmpPluginPath,true);
			File.Delete(npluginzFilePath);
		}

        #endregion
    };
}
