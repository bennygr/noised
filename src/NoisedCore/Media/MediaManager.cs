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
			//TODO: Getting the real audio plugin for
			//the URI
			var audio = pluginLoader.GetPlugin<IAudioPlugin>();
			audio.Play(item);
		}
		
		#endregion
	};
}
