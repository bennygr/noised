using System;
using Noised.Core;
using Noised.Core.Config;
using Noised.Core.DB;
using Noised.Core.IOC;
using Noised.Core.Media;
using Noised.Core.Plugins;
using Noised.Core.Service;
using Noised.Core.UserManagement;
using Noised.Logging;

namespace Noised.Server
{
    public class Start
    {
        public static int Main(string[] args)
        {
            IocContainer.Build();
            ILogging logger = IocContainer.Get<ILogging>();
            logger.AddLogger(new ConsoleLogger());

            if (args != null && args.Length != 0)
                return HandleArgs(args);

            logger.Info("Hello I am Noised - your friendly music player daemon");

            //Creating the DB or update if neccessary
            var db = IocContainer.Get<IDB>();
            db.CreateOrUpdate();

            //installing new plugins
            var pluginInstaller = IocContainer.Get<IPluginInstaller>();
            pluginInstaller.InstallAll("./plugins");

            //Loading configuration
            var config = IocContainer.Get<IConfig>();
            config.Load(IocContainer.Get<IConfigurationLoader>());

            //loading plugins
            logger.Info("Loading plugins:");
            var pluginLoader = IocContainer.Get<IPluginLoader>();
            int pluginCount = pluginLoader.LoadPlugins();
            logger.Info(pluginCount + " plugins loaded ");

            //Add a factory and create a ping command
            var core = IocContainer.Get<ICore>();
            core.Start();

            //Starting services
            var serviceConnectionManager = IocContainer.Get<IServiceConnectionManager>();
            serviceConnectionManager.StartServices();

            // Refreshing music
            logger.Info("Refreshing music...");
            var sourceAccumulator = IocContainer.Get<IMediaSourceAccumulator>();
            sourceAccumulator.Refresh();
            logger.Info("Done refreshing music.");

            // Initializing Playlistmanager
            logger.Info("Initializing Playlistmanager");
            IocContainer.Get<IPlaylistManager>().RefreshPlaylists();
            logger.Info("Done initializing Playlistmanager");

            logger.Info("Noised has been started.");

            return 0;
        }

        private static int HandleArgs(string[] args)
        {
            ILogging logger = IocContainer.Get<ILogging>();

            if (args.Length == 1)
            {
                if (args[0].StartsWith("CreateUser="))
                {
                    string realArgs = args[0].Split('=')[1];

                    if (String.IsNullOrWhiteSpace(realArgs))
                    {
                        logger.Error("CreateUser command was invoked without a parameter");
                        return -2;
                    }

                    string[] realArgArr = realArgs.Split(',');

                    IocContainer.Get<IUserManager>().CreateUser(realArgArr[0], realArgArr[1]);

                    logger.Info("\"" + realArgArr[0] + "\" can now rock this noised!\nPress Enter to continue!");
                    Console.ReadLine();

                    return 0;
                }
            }

            logger.Error("NoisedServer was invoked with an invalid number of arguments or an unknown argument");
            return -1;
        }
    }
}
