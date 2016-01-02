using System;
using System.Collections.Generic;
using Noised.Logging;
using Noised.Core.IOC;
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

        public void Refresh()
        {
            foreach (IMediaSource mediaSource in pluginLoader.GetPlugins<IMediaSource>())
            {
				IocContainer.Get<ILogging>().Info(String.Format("Refreshing media source {0}...",
																mediaSource.Identifier));
				DateTime t0 = DateTime.Now;
                mediaSource.Refresh();
				DateTime t1 = DateTime.Now;
				TimeSpan td = t1 - t0;
				IocContainer.Get<ILogging>().Info(String.Format("Refreshed media source {0} in {1} seconds",
																mediaSource.Identifier,
																td.TotalSeconds));
            }
        }

		public MediaItem Get(Uri uri)
		{
            foreach (var source in pluginLoader.GetPlugins<IMediaSource>())
            {
				var ret =source.Get(uri);
				if(ret != null)
				{
					return ret;
				}
			}
			return null;
		}

        public IEnumerable<MediaSourceSearchResult> Search(string pattern)
        {
            var ret = new List<MediaSourceSearchResult>();
            foreach (var source in pluginLoader.GetPlugins<IMediaSource>())
            {
                ret.Add(source.Search(pattern));
            }
            return ret;
        }

        #endregion
    };
}
