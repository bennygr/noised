using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Noised.Logging;
using Noised.Core.Plugins;
using Noised.Core.Plugins.Commands;

namespace Noised.Core.Commands
{
    /// <summary>
    ///		A factory creating commands
    /// </summary>
    class CommandFactory : ICommandFactory
    {
        private readonly IPluginLoader pluginLoader;
        private readonly ILogging logging;

        /// <summary>
        ///		Constructor
        /// </summary>
        public CommandFactory(IPluginLoader pluginLoader, ILogging logging)
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
            if (name == null)
                throw new ArgumentNullException("name");
            IEnumerable<ICommandBundle> commandBundles = 
                pluginLoader.GetPlugins<ICommandBundle>();
            AbstractCommand command = null;
            if (commandBundles.Any())
            {
                foreach (var commandBundle in commandBundles)
                {
                    Assembly assembly = Assembly.GetAssembly(commandBundle.GetType());
                    if (assembly != null)
                    {
                        Type commandType = assembly.GetType(name);
                        if (commandType != null)
                        {
                            if (parameters == null || parameters.Length == 0)
                            {
                                command = (AbstractCommand)Activator.CreateInstance(commandType);
                            }
                            else
                            {
                                command = (AbstractCommand)Activator.CreateInstance(commandType, parameters);
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
    }
};
