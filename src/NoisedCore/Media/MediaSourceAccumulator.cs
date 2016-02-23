using System;
using System.Collections.Generic;
using System.Linq;
using Noised.Core.IOC;
using Noised.Core.Plugins;
using Noised.Core.Plugins.Media;
using Noised.Logging;

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
                var ret = source.Get(uri);
                if (ret != null)
                    return ret;
            }
            return null;
        }

        /// <summary>
        /// Searches all known IMediaSources for the given search pattern
        /// </summary>
        /// <param name="pattern">The search pattern</param>
        /// <returns>
        /// An enumeration of search results matching the given pattern
        /// </returns>
        public IEnumerable<MediaSourceSearchResult> Search(string pattern)
        {
            return Search(pattern, null);
        }

        /// <summary>
        /// Searches a set of IMediaSources for the given search pattern
        /// </summary>
        /// <param name="pattern">The search pattern</param>
        /// <param name="sourceIdentifiers">The identifiers of the IMediaSources to search in</param>
        /// <returns>
        /// An enumeration of search results matching the given pattern
        /// </returns>
        public IEnumerable<MediaSourceSearchResult> Search(string pattern, IEnumerable<string> sourceIdentifiers)
        {
            List<MediaSourceSearchResult> ret = new List<MediaSourceSearchResult>();

            foreach (IMediaSource source in pluginLoader.GetPlugins<IMediaSource>())
            {
                if (sourceIdentifiers != null &&
                    sourceIdentifiers.Any() &&
                    !sourceIdentifiers.Contains(source.Identifier))
                    continue;

                ret.Add(source.Search(pattern));
            }

            return ret;
        }

        /// <summary>
        /// Gets all Sources
        /// </summary>
        public IEnumerable<string> Sources
        {
            get
            {
                return pluginLoader.GetPlugins<IMediaSource>().Select(mediaSource => mediaSource.Identifier);
            }
        }

        #endregion
    };
}
