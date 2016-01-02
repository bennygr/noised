using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Noised.Logging;
using Noised.Core.DB;
using Noised.Core.IOC;

namespace Noised.Core.Plugins
{
    /// <summary>
    ///		Basic plugin loader
    /// </summary>
    public class PluginLoader : IPluginLoader
    {
        #region Fields

        private const String PLUGIN_TYPE_NAME = "Noised.Core.Plugins.IPlugin";
        private readonly List<IPlugin> plugins = new List<IPlugin>();
        private readonly Dictionary<IPlugin,PluginMetaData> metaDataStore = new Dictionary<IPlugin,PluginMetaData>();

        #endregion

        #region IPluginLoader

        public int LoadPlugins(string localPluginPath)
        {
			var logger = IocContainer.Get<ILogging>();	
            var files = Directory.GetFiles(localPluginPath).Where(
                   file => file.EndsWith(".nplugin", StringComparison.Ordinal));
            string currentFileName;
            foreach (var file in files)
            {
                currentFileName = file;
                IEnumerable<Type> pluginTypes;
                try
                {
                    //Getting IPlugin types from the plugin assembly 
                    Assembly assembly = Assembly.LoadFrom(file);

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
                        using (var unitOfWork = IocContainer.Get<IUnitOfWork>())
                        {
                            metaData = unitOfWork.PluginRepository.GetForFile(new FileInfo(file));	
                            if (metaData != null)
                            {
                                metaDataStore.Add(plugin, metaData);
                            }
                            else
                            {
                                throw new Exception("No PluginMetaData found for file " + file);
                            }
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
                        "Could not load plugin " + currentFileName);
                    IocContainer.Get<ILogging>().Error(e.Message);
                }
            }
            return plugins.Count;
        }
		
        public IEnumerable<IPlugin> GetPlugins()
        {
            return plugins;
        }
		
        public IEnumerable<T> GetPlugins<T>()
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
		
        public T GetPlugin<T>()
        {
            IPlugin p = plugins.Find(plugin => plugin is T);
            if (p != null)
                return (T)p;
            return default(T);
        }

		public PluginMetaData GetMetaData(IPlugin plugin)
		{
			PluginMetaData ret;
			metaDataStore.TryGetValue(plugin,out ret);
			return ret;
		}

        #endregion
    };
}
