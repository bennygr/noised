using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Noised.Logging;
using Noised.Core.Plugins;
using Noised.Core.Plugins.Commands;
using Noised.Core.Service.Protocols;

namespace Noised.Core.Commands
{
	/// <summary>
	///		A factory creating commands
	/// </summary>
	internal class CommandFactory : ICommandFactory
	{
		private IPluginLoader pluginLoader;
		private ILogging logging;

		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="The plugin loader"></param>
		public CommandFactory(IPluginLoader pluginLoader,ILogging logging)
		{
			this.pluginLoader = pluginLoader;
			this.logging = logging;
		}

		public AbstractCommand CreateCommand(CommandMetaData commandMetaData)
		{
			return CreateCommand(commandMetaData.Name,
								 commandMetaData.Parameters == null ? 
								 null : 
								 commandMetaData.Parameters.ToArray());
		}

		/// <summary>
		///		Creates a command
		/// </summary>
		/// <param name="name">The full name of the command to create</param>
		/// <param name="parameters">The parameters required for the command creation</param>
		public AbstractCommand CreateCommand(string name, params object[] parameters)
		{
			if(name == null)
				throw new ArgumentNullException("Name");
			IEnumerable<ICommandBundle> commandBundles = 
				pluginLoader.GetPlugins<ICommandBundle>();
			AbstractCommand command = null;
			if(commandBundles.Count() > 0)
			{
				foreach(var commandBundle in commandBundles)
				{
					Assembly assembly = Assembly.GetAssembly(commandBundle.GetType());
					if(assembly != null)
					{
						Type commandType = assembly.GetType(name);
						if(commandType != null)
						{
							if(parameters == null || parameters.Length == 0)
							{
								command = (AbstractCommand)Activator.CreateInstance(commandType);
							}
							else
							{
								command = (AbstractCommand)Activator.CreateInstance(commandType,parameters);
							}
						}
						else
						{
							logging.Error(String.Format("Could not find type for command {0}",
														name));
						}
					}
				}
			}
			else
			{
				logging.Error(
						String.Format("Could not create command {0}, because no command plugin was loaded",
									  name));
			}
			logging.Debug("CommandFactory created command: " + command);
			return command;
		}
	};
}
