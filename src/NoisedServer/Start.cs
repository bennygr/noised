using System;
using System.Threading;

using Noised.Core.Plugins.Audio;
//TODO: remove
using Noised.Plugins.Audio.GStreamer;
namespace Noised.Server
{
	public class Start
	{
		public static int Main(String[] args)
		{
			Console.WriteLine("Hello Noised");
			//Just a temporary instantiating a concrete plugin
			//for testing purpose
			//TODO: change
			IAudioPlugin audioPlugin = 
				new GStreamerAudioPlugin();
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
