using System;
using System.Linq;
using Noised.Core;
using Noised.Core.Config;
using Noised.Core.DB;
using Noised.Core.IOC;
using Noised.Core.Media;
using Noised.Core.Plugins;
using Noised.Core.Plugins.Audio;
using Noised.Core.Plugins.Media;
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

            var mediaSource = pluginLoader.GetPlugin<IMediaSource>();
            if (mediaSource != null)
            {
                try
                {
                    var audioPlugin = pluginLoader.GetPlugin<IAudioPlugin>();
                    audioPlugin.SongFinished +=
                        (sender, mediaItem) =>
                        Console.WriteLine("SONG HAS BEEN FINISHED. I WANT MORE MUSIC :-)");
                    var resultList = mediaSource.Search("test");
                    MediaItem test = resultList.First();
                    Console.WriteLine(test.Protocol);

                    //IocContainer.Get<IMediaManager>().Play(test);
                    //Thread.Sleep(5000);
                    //IocContainer.Get<IMediaManager>().Pause();
                    //Thread.Sleep(3000);
                    //IocContainer.Get<IMediaManager>().Resume();
                    //Thread.Sleep(3000);
                    //IocContainer.Get<IMediaManager>().Stop();
                }
                catch (Exception e)
                {
                    logger.Error(e.Message);
                }
            }
            else
            {
                logger.Error("No media source plugin found");
            }
            return 0;
        }
    }
}
