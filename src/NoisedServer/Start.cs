using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using Noised.Logging;
using Noised.Core.Plugins;
using Noised.Core.Plugins.Audio;
//TODO: remove
//using Noised.Plugins.Audio.GStreamer;
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
			
			IAudioPlugin audioPlugin = 
				pluginLoader.GetPlugin<IAudioPlugin>();
			if(audioPlugin != null)
			{
				audioPlugin.Play("file:///home/bgr/Musik/test.mp3");
				Console.WriteLine("Playing");
				Thread.Sleep(3000);
				audioPlugin.Pause();
				Console.WriteLine("Pause");
				Thread.Sleep(3000);
				audioPlugin.Resume();
				Console.WriteLine("Playing");
				Thread.Sleep(3000);
				audioPlugin.Stop();
				Console.WriteLine("Disposing Plugin");
				audioPlugin.Dispose();
			}
			else
			{
				logger.Error("No audio plugin found");
			}
			return 0;
		}
	};
}
