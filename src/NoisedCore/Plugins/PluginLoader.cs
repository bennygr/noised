using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Noised.Logging;

namespace Noised.Core.Plugins
{
	/// <summary>
	///		Basic plugin loader
	/// </summary>
	public class PluginLoader : IPluginLoader
	{
		#region Fields
		
		private List<IPlugin> plugins = new List<IPlugin>();
		
		#endregion

		#region IPluginLoader
		
		public int LoadPlugins(string localPluginPath)
		{
			var files = Directory.GetFiles(localPluginPath).Where(
							file => file.EndsWith(".dll") );
			foreach(var file in files)
			{
				IEnumerable<Type> pluginTypes = null;
				try
				{
					//Getting IPlugin types from the plugin assembly 
					Assembly assembly = Assembly.LoadFrom(file);
					String strPluginType = "Noised.Core.Plugins.IPlugin";
					Type pluginBaseType = Type.GetType(strPluginType);
					pluginTypes = assembly.GetTypes().Where(type => 
														    pluginBaseType.IsAssignableFrom(type));
				}
				catch(Exception e)
				{
					IocContainer.Get<ILogging>().Error(e.Message);
				}
		
				if(pluginTypes != null && pluginTypes.Any())
				{	
					//Plugin init data
					PluginInitializer pluginInitializer = 
						new PluginInitializer()
						{
							Logging =  IocContainer.Get<ILogging>()
						};
					//Instantiate the first IPlugin type found
					Type concreteType = pluginTypes.First();
					IPlugin plugin = (IPlugin)Activator.CreateInstance(concreteType,pluginInitializer);
					plugins.Add(plugin);
					IocContainer.Get<ILogging>().Debug(
							String.Format("Loaded Plugin {0} - {1}",
										  plugin.Name, 
									      plugin.Description));
				}
				else
				{
					IocContainer.Get<ILogging>().Error(
							String.Format("No IPlugin implementation found in assembly {0}",file));
				}
			}
		
			return plugins.Count;
		}
		
		public IEnumerable<IPlugin> GetPlugins()
		{
			return this.plugins;
		}
		
		public IEnumerable<T> GetPlugins<T>()
		{
			List<T> concretPlugins = new List<T>();
			foreach(var plugin in plugins)
			{
				if(typeof(T).IsAssignableFrom(plugin.GetType()))
				{
					concretPlugins.Add((T)plugin);
				}
			}
			return concretPlugins;
		}
		
		public T GetPlugin<T>()
		{
			IPlugin p = this.plugins.Find( plugin => typeof(T).IsAssignableFrom(plugin.GetType()) );
			if(p != null)
				return (T)p;
			return default(T);
		}
		
		#endregion
	};
}
