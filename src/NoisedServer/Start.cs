using System;
using System.Threading;
using System.Collections.Generic;
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
			Logger.AddLogger(new ConsoleLogger());

			Logger.Debug("Hello Noised");
			IPluginLoader pluginLoader = new PluginLoader();
			int pluginCount = pluginLoader.LoadPlugins("./plugins");
			Logger.Debug(pluginCount + " plugins loaded ");
			
			IAudioPlugin audioPlugin = 
				pluginLoader.GetPlugin<IAudioPlugin>();
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
			return 0;
		}
	};
}
