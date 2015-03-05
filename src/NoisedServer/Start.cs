using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using Noised.Logging;
using Noised.Core.IOC;
using Noised.Core.Plugins;
using Noised.Core.Plugins.Audio;
using Noised.Core;
using Noised.Core.Commands;
using Noised.Core.Service;


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


			User user = new User("Benny");
			//Add a factory and create a ping command
			Noised.Core.Core core = new Noised.Core.Core();
			core.Start();
			//core.AddCommand(new PingCommand(user));

			ServiceHandler serviceHandler = new ServiceHandler();
			serviceHandler.StartServices();
			
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
