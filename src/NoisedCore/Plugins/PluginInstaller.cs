using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Noised.Core.DB;
using Noised.Core.IOC;
using Noised.Logging;

namespace Noised.Core.Plugins
{
    /// <summary>
    ///		Default plugin installer
    /// </summary>
    public class PluginInstaller : IPluginInstaller
    {
        /// <summary>
        ///		Extracts the content of a ziped plugin file (npluginz file) to a new tmp directory
        /// </summary>
        /// <param name="noisedTmpRoot">The root tmp folder of noised</param>
        /// <param name="npluginzFilePath">The full path to the ziped plugin file"></param>
        /// <returns>The full path to the new plugin's temporary directory</returns>
        private string ExtractPlugin(string noisedTmpRoot, string npluginzFilePath)
        {
            //Creating a tmp dir 
            string tmpPluginPath = noisedTmpRoot + Path.DirectorySeparatorChar + Guid.NewGuid();
            IocContainer.Get<ILogging>().Debug(String.Format("Unpacking new plugin {0}...", npluginzFilePath));
            //..and extract the plugin content to the dir
            System.IO.Compression.ZipFile.ExtractToDirectory(npluginzFilePath, tmpPluginPath);
            IocContainer.Get<ILogging>().Debug(
                String.Format("Plugin {0} has been extracted to {1}", npluginzFilePath, tmpPluginPath));
            return tmpPluginPath;
        }

        /// <summary>
        ///		Reads the plugin meta data from the metadata file (plugin.nplugininfo)
        /// </summary>
        /// <returns>The plugin meta data read from file</returns>
        private PluginMetaData ReadPluginMetaDataFromFile(string metaDataFile)
        {
            if (!File.Exists(metaDataFile))
            {
                throw new FileNotFoundException("Plugin does not contain a plugin.nplugininfo file");
            }
            string metaContent = File.ReadAllText(metaDataFile);
            return JsonConvert.DeserializeObject<PluginMetaData>(metaContent);
        }

        /// <summary>
        ///		Checks whether a plugin is currently installed or not
        /// </summary>
        /// <param name="pluginMetaData">The plugin'n meta data</param>
        /// <returns>True if the plugin is already installed, false otherwise</returns>
        private bool IsPluginInstalled(PluginMetaData pluginMetaData)
        {
            using (IUnitOfWork unitOfWork = IocContainer.Get<IUnitOfWork>())
            {
                var existingPlugin = unitOfWork.PluginRepository.GetByGuid(pluginMetaData.GetGuid());
                return (existingPlugin != null && existingPlugin.GetVersion() >= pluginMetaData.GetVersion());
            }
        }

        #region IPluginInstaller implementation

        public void InstallAll(string path)
        {
            var files = Directory.GetFiles(path).Where(
                            file => file.EndsWith(".npluginz", StringComparison.Ordinal));

            //Extract all nplugins file to the tmp directory and process them from there
            foreach (var file in files)
            {
                try
                {
                    IocContainer.Get<ILogging>().Debug(String.Format("Found new plugin {0}...", file));
                    Install(file);
                }
                catch (Exception e)
                {
                    IocContainer.Get<ILogging>().Error(String.Format("Error while loading plugin {0}: {1}", file, e.Message));
                    IocContainer.Get<ILogging>().Debug(e.StackTrace);
                }
            }
        }

        public void Install(string npluginzFilePath)
        {
            var logger = IocContainer.Get<ILogging>();

            //Create a tmp if not existing yet directory
            string tmpPath = "." + Path.DirectorySeparatorChar + "tmp";
            if (!Directory.Exists(tmpPath))
            {
                Directory.CreateDirectory(tmpPath);
            }

            //Extract the plugin to a new tmp directory within the noised tmp directory
            string tmpPluginDirectory = ExtractPlugin(tmpPath, npluginzFilePath);

            //Getting the meta data from the plugin which should be installed
            string metaDataFile = tmpPluginDirectory + Path.DirectorySeparatorChar + "plugin.nplugininfo";
            var pluginRegistrationData = ReadPluginMetaDataFromFile(metaDataFile);

            bool isPluginInstalled = IsPluginInstalled(pluginRegistrationData);
            //Installing the plugin if not yet installed or if marked as developer version (0.0.0) (usefull for testing)
            Version developVersion = new Version(0, 0, 0);
            bool forceInstallation = pluginRegistrationData.GetVersion().Equals(developVersion);
            if (isPluginInstalled && forceInstallation)
            {
                logger.Debug(String.Format(
                        "Plugin {0} has develop version of 0.0.0. Forcing Installation...",
                        pluginRegistrationData.Name));
            }

            if (!isPluginInstalled || forceInstallation)
            {
                //Installing plugin
                using (IUnitOfWork unitOfWork = IocContainer.Get<IUnitOfWork>())
                {
                    //Uninstall old files first (if existing)
                    if (unitOfWork.PluginRepository.GetByGuid(pluginRegistrationData.GetGuid()) != null)
                    {
                        logger.Debug(String.Format("Updating plugin {0}...", npluginzFilePath));
                        logger.Debug(String.Format("Uninstalling old version of plugin {0}...", npluginzFilePath));
                        unitOfWork.PluginRepository.UnregisterPlugin(pluginRegistrationData);
                        logger.Debug(String.Format("Old version of plugin {0} has been removed", npluginzFilePath));
                    }

                    logger.Debug(String.Format("Installing new plugin {0}...", npluginzFilePath));

                    //Install the plugin
                    //Installing plugin configuration  
                    var etcPath = new DirectoryInfo(tmpPluginDirectory + Path.DirectorySeparatorChar + "etc");
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
                        File.Copy(etcFile.FullName, installedFile.FullName, true);
                        installedFiles.Add(installedFile);
                    }
                    //Installing plugin files  
                    var pluginsPath = new DirectoryInfo(tmpPluginDirectory + Path.DirectorySeparatorChar + "plugins");
                    string noisedPluginsPath =
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                        Path.DirectorySeparatorChar +
                        "plugins";
                    foreach (var pluginFile in pluginsPath.GetFiles())
                    {
                        //Copy plugin file
                        var installedFile = new FileInfo(noisedPluginsPath + Path.DirectorySeparatorChar + pluginFile.Name);
                        File.Copy(pluginFile.FullName, installedFile.FullName, true);
                        installedFiles.Add(installedFile);
                    }

                    //Registering the plugin into the DB
                    unitOfWork.PluginRepository.RegisterPlugin(pluginRegistrationData, installedFiles);
                    IocContainer.Get<ILogging>().Debug(
                        String.Format("Plugin {0} has been installed (Version {1})",
                            pluginRegistrationData.Name,
                            pluginRegistrationData.Version));

                    unitOfWork.SaveChanges();
                }
            }
            else
            {
                logger.Debug(String.Format("Plugin {0} is already installed with version {1} or newer",
                        pluginRegistrationData.Name,
                        pluginRegistrationData.Version));
            }

            //Clean up
            Directory.Delete(tmpPluginDirectory, true);
            File.Delete(npluginzFilePath);
        }

        #endregion
    };
}
