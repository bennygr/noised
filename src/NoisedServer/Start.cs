using System;
using System.Linq;
using Noised.Core;
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

            logger.Debug("Hello Noised");
            IPluginLoader pluginLoader = IocContainer.Get<IPluginLoader>();
            int pluginCount = pluginLoader.LoadPlugins("./plugins");
            logger.Debug(pluginCount + " plugins loaded ");

            //Add a factory and create a ping command
            ICore core = IocContainer.Get<ICore>();
            core.Start();

            ServiceConnectionManager serviceConnectionManager = new ServiceConnectionManager();
            serviceConnectionManager.StartServices();

            IMediaSource mediaSource = pluginLoader.GetPlugin<IMediaSource>();
            if (mediaSource != null)
            {
                try
                {
					IAudioPlugin audioPlugin = 
						pluginLoader.GetPlugin<IAudioPlugin>();
					audioPlugin.SongFinished += 
						(sender,mediaItem) => 
						Console.WriteLine("SONG HAS BEEN FINISHED. I WANT MORE MUSIC :-)");
                    var resultList = mediaSource.Search("test");
                    MediaItem test = resultList.First();
                    Console.WriteLine(test.Protocol);
                    IocContainer.Get<IMediaManager>().Play(test);
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
    };
}
