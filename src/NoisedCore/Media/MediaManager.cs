using System.Linq;
using Noised.Core.Plugins;
using Noised.Core.Plugins.Audio;
namespace Noised.Core.Media
{
	public class MediaManager : IMediaManager
	{
		IPluginLoader pluginLoader;

		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="pluginLoader">Pluginloader</param>
		public MediaManager (IPluginLoader pluginLoader)
		{
			this.pluginLoader = pluginLoader;
		}

		#region IMediaManager
		
		public bool Shuffle {get;set;}
		
		public bool Repeat {get;set;}
		
		public void Play(MediaItem item)
		{
			var audio = pluginLoader.GetPlugin<IAudioPlugin>();
			audio.Play(item);
		}

		public void Stop()
		{
			var audios = pluginLoader.GetPlugins<IAudioPlugin>();
			audios.ToList().ForEach(audio => audio.Stop());
		}

		public void Pause()
		{
			var audios = pluginLoader.GetPlugins<IAudioPlugin>();
			audios.ToList().ForEach(audio => audio.Pause());
		}

		public void Resume()
		{
			var audios = pluginLoader.GetPlugins<IAudioPlugin>();
			audios.ToList().ForEach(audio => audio.Pause());
		}
		
		#endregion
	};
}
