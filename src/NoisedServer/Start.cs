using System;
using System.Threading;
using Noised.Core;
using Noised.Core.Config;
using Noised.Core.DB;
using Noised.Core.IOC;
using Noised.Core.Media;
using Noised.Core.Plugins;
using Noised.Core.Plugins.Audio;
using Noised.Core.Service;
using Noised.Logging;

namespace Noised.Server
{
    public class Start
    {
        public static int Main()
        {
            IocContainer.Build();

            ILogging logger = IocContainer.Get<ILogging>();
            logger.AddLogger(new ConsoleLogger());

            logger.Debug("Hello I am Noised - your friendly music player daemon");
			
            //Loading configuration
            var config = IocContainer.Get<IConfig>();
            config.Load(IocContainer.Get<IConfigurationLoader>());

            //Creating the DB or update if neccessary
            var db = IocContainer.Get<IDB>();
            db.CreateOrUpdate();
			
            //installing new plugins
            var pluginInstaller = IocContainer.Get<IPluginInstaller>();
            pluginInstaller.InstallAll("./plugins");

            //loading plugins
            var pluginLoader = IocContainer.Get<IPluginLoader>();
            int pluginCount = pluginLoader.LoadPlugins("./plugins");
            logger.Debug(pluginCount + " plugins loaded ");

            //Add a factory and create a ping command
            var core = IocContainer.Get<ICore>();
            core.Start();

            //Starting services
            var serviceConnectionManager = new ServiceConnectionManager();
            serviceConnectionManager.StartServices();

            //Test:
            //Checking sources for new Music
            logger.Debug("Refreshing music...");
            var sourceAccumulator = IocContainer.Get<IMediaSourceAccumulator>();
            sourceAccumulator.Refresh();
            logger.Debug("Done refreshing music.");
			
            var sources = IocContainer.Get<IMediaSourceAccumulator>();
            try
            {
                var audioPlugin = pluginLoader.GetPlugin<IAudioPlugin>();
                audioPlugin.SongFinished += 
						(sender, mediaItem) => 
						Console.WriteLine("SONG HAS BEEN FINISHED. I WANT MORE MUSIC :-)");
				string testUri = @"file:///home/bgr/Musik/AFI/I Heard a Voice/AFI - 06 - The Days Of The Phoenix (Live Arena Long Beach CA).mp3";
                var testItem = sources.Get(new Uri(testUri));
                if (testItem == null)
                    logger.Debug("NOT FOUND!!");
                else
                {
                    logger.Debug(testItem.ToString());
                    IocContainer.Get<IMediaManager>().Play(testItem);
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
            }
            return 0;
        }
    }
}
