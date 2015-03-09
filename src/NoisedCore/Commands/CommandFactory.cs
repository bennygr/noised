using System;
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

		public IEnumerable<AbstractCommand> CreateCommands(CommandMetaDataContainer commandMetaDataContainer)
		{
			Console.WriteLine("creating commands: " + commandMetaDataContainer.Commands.Count);
			foreach(var commandMetaData in commandMetaDataContainer.Commands)
				yield return CreateCommand(commandMetaData.Name,
										   commandMetaData.Parameters.ToArray());
		}

		/// <summary>
		///		Creates a command
		/// </summary>
		/// <param name="name">The full name of the command to create</param>
		/// <param name="parameters">The parameters required for the command creation</param>
		public AbstractCommand CreateCommand(string name, params object[] parameters)
		{
			IEnumerable<ICommandBundle> commandBundles = 
				pluginLoader.GetPlugins<ICommandBundle>();
			foreach(var commandBundle in commandBundles)
			{
				Assembly assembly = Assembly.GetAssembly(commandBundle.GetType());
				if(assembly != null)
				{
					Type commandType = assembly.GetType(name);
					if(commandType != null)
						Console.WriteLine(assembly.ToString());
				}
			}
			return null;
		}
	};
}
