using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Noised.Core.DB;
using Noised.Core.IOC;
using Noised.Logging;

namespace Noised.Core.Plugins
{
    /// <summary>
    ///		Basic plugin loader
    /// </summary>
    public class PluginLoader : IPluginLoader
    {
        #region Fields

        private const String PLUGIN_TYPE_NAME = "Noised.Core.Plugins.IPlugin";
        private List<IPlugin> plugins = new List<IPlugin>();
        private readonly Dictionary<IPlugin, PluginMetaData> metaDataStore = new Dictionary<IPlugin, PluginMetaData>();

        #endregion

        #region IPluginLoader

        public int LoadPlugins()
        {
            var logger = IocContainer.Get<ILogging>();
            using (var unitOfWork = IocContainer.Get<IUnitOfWork>())
            {
                var files = unitOfWork.PluginRepository.GetFiles(".nplugin");
                FileInfo currentFile = null;
                foreach (var file in files)
                {
                    currentFile = file;
                    IEnumerable<Type> pluginTypes;
                    try
                    {
                        //Getting IPlugin types from the plugin assembly 
                        Assembly assembly = Assembly.LoadFrom(file.FullName);

                        Type pluginBaseType = Type.GetType(PLUGIN_TYPE_NAME);
                        pluginTypes = assembly.GetTypes().Where(pluginBaseType.IsAssignableFrom);
                        if (pluginTypes != null && pluginTypes.Any())
                        {
                            //Plugin init data
                            var pluginInitializer =
                                new PluginInitializer
                                {
                                    Logging = logger
                                };
                            //Instantiate the first IPlugin type found
                            Type concreteType = pluginTypes.First();
                            var plugin = (IPlugin)Activator.CreateInstance(concreteType, pluginInitializer);
                            plugins.Add(plugin);

                            //Loading meta data
                            PluginMetaData metaData = null;
                            metaData = unitOfWork.PluginRepository.GetForFile(file);
                            if (metaData != null)
                            {
                                metaDataStore.Add(plugin, metaData);
                            }
                            else
                            {
                                throw new Exception("No PluginMetaData found for file " + file);
                            }
                            IocContainer.Get<ILogging>().Info(
                                String.Format("Loaded Plugin {0} - {1}",
                                    metaData.Name,
                                    metaData.Description));

                        }
                        else
                        {
                            IocContainer.Get<ILogging>().Error(
                                String.Format("No IPlugin implementation found in assembly {0}", file));
                        }
                    }
                    catch (Exception e)
                    {
                        IocContainer.Get<ILogging>().Error(
                            "Could not load plugin " + currentFile.FullName);
                        IocContainer.Get<ILogging>().Error(e.Message);
                    }
                }
                plugins = plugins.OrderByDescending(p => p.GetMetaData().Priority).ToList();
                return plugins.Count;
            }
        }

        public IEnumerable<IPlugin> GetPlugins()
        {
            return plugins;
        }

        public IEnumerable<T> GetPlugins<T>() where T : IPlugin
        {
            var concretPlugins = new List<T>();
            foreach (var plugin in plugins)
            {
                if (plugin is T)
                {
                    concretPlugins.Add((T)plugin);
                }
            }
            return concretPlugins;
        }

        public T GetPlugin<T>() where T : IPlugin
        {
            IPlugin plugin = GetPlugins<T>().OrderByDescending(p => p.GetMetaData().Priority).FirstOrDefault();
            if (plugin != null)
                return (T)plugin;
            return default(T);
        }

        public PluginMetaData GetMetaData(IPlugin plugin)
        {
            PluginMetaData ret;
            metaDataStore.TryGetValue(plugin, out ret);
            return ret;
        }

        #endregion
    };
}
