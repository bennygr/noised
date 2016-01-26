using System.Linq;
using Noised.Core;
using Noised.Core.Config;
using Noised.Core.DB;
using Noised.Core.IOC;
using Noised.Core.Media;
using Noised.Core.Plugins;
using Noised.Core.Service;
using Noised.Logging;
using Noised.Plugins.Commands.CoreCommands;

namespace Noised.Server
{
    public class Start
    {
        public static int Main()
        {
            IocContainer.Build();
            ILogging logger = IocContainer.Get<ILogging>();
            logger.AddLogger(new ConsoleLogger());
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

            //Add a factory and create a ping command ???
            var core = IocContainer.Get<ICore>();
            core.Start();

            //Starting services
            var serviceConnectionManager = new ServiceConnectionManager();
            serviceConnectionManager.StartServices();

            // Refreshing music
            logger.Info("Refreshing music...");
            var sourceAccumulator = IocContainer.Get<IMediaSourceAccumulator>();
            sourceAccumulator.Refresh();
            logger.Info("Done refreshing music.");

            logger.Info("Noised has been started.");

            // ---------- Testcode ----------
            IocContainer.Get<IPlaylistManager>().AddPlaylist(IocContainer.Get<IPlaylistManager>().CreatePlaylist("testliste"));
            sourceAccumulator.Search("%")
                .ToList()
                .ForEach(
                    x => x.MediaItems.ToList().ForEach(y => IocContainer.Get<IPlaylistManager>().FindPlaylists("testliste").Add(y)));

            core.ExecuteCommandAsync(new Play(null, @"C:\test\test2.mp3"));
            
            return 0;
        }
    }
}
