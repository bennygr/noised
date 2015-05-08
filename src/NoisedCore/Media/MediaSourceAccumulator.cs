using System;
using System.Collections.Generic;
using Noised.Core.Plugins;
using Noised.Core.Plugins.Media;

namespace Noised.Core.Media
{
	/// <summary>
	///		Default implementation of an SourceManager which
	///		accumulates all IMediaSource's
	/// </summary>
	public class MediaSourceAccumulator : IMediaSourceAccumulator
	{
		private readonly IPluginLoader pluginLoader;

		public MediaSourceAccumulator(IPluginLoader pluginLoader)
		{
			this.pluginLoader = pluginLoader;
		}

		#region IMediaSourceManager
		
		public MediaItem GetItem(Uri uri)
		{
			foreach(IMediaSource sourcePlugin  in
					pluginLoader.GetPlugins<IMediaSource>())
			{
				MediaItem item = sourcePlugin.GetItem(uri);
				if(item != null)
					return item;
			}
			return null;
		}
		
		public IEnumerable<MediaItem> Search(string search)
		{
			var ret = new List<MediaItem>();
			foreach(IMediaSource sourcePlugin  in
					pluginLoader.GetPlugins<IMediaSource>())
			{
				ret.AddRange(sourcePlugin.Search(search));
			}
			return ret;
		}
		
		#endregion
	};
}
