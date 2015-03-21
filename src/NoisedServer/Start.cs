using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using Noised.Core;
using Noised.Logging;
using Noised.Core.IOC;
using Noised.Core.Plugins;
using Noised.Core.Plugins.Audio;
using Noised.Core.Commands;
using Noised.Core.Service;
using Noised.Core.Media;

namespace Noised.Server
{
	public class Start
	{
		public static int Main(String[] args)
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
			
			IAudioPlugin audioPlugin = 
				pluginLoader.GetPlugin<IAudioPlugin>();
			if(audioPlugin != null)
			{
				MediaItem test = new MediaItem()
				{
					Uri= new Uri("file:///home/bgr/Musik/test.mp3")
				};
				audioPlugin.Play(test);
			}
			else
			{
				logger.Error("No audio plugin found");
			}
			return 0;
		}
	};
}
