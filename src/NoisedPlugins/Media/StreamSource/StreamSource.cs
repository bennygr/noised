﻿using System;
using System.Collections.Generic;
using System.Linq;
using Noised.Core.Media;
using Noised.Core.Plugins;
using Noised.Core.Plugins.Media;

namespace Noised.Plugins.Media.StreamSource
{
    public class StreamSource : IMediaSource
    {
        private readonly List<string> streamUris;

        public StreamSource(PluginInitializer initalizer)
        {
            streamUris = new List<string> { "http://mp3channels.webradio.antenne.de/rockantenne" };
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Führt anwendungsspezifische Aufgaben aus, die mit dem Freigeben, Zurückgeben oder Zurücksetzen von nicht verwalteten Ressourcen zusammenhängen.
        /// </summary>
        public void Dispose()
        { }

        #endregion

        #region Implementation of IMediaSource

        /// <summary>
        ///		A unique name of the media source
        /// </summary>
        public string Identifier
        {
            get
            {
                return "STREAM";
            }
        }

        /// <summary>
        ///		Initializes and/or refreshs the media source
        /// </summary>
        public void Refresh()
        { }

        /// <summary>
        ///		Gets the media item for the given URI
        /// </summary>
        /// <param name="uri">The uri</param>
        /// <returns>The MediaItem for the given Uri, or null if no such MediaItem was found</returns> 
        public MediaItem Get(Uri uri)
        {
            if (streamUris.Any(x => x == uri.AbsoluteUri))
                return new MediaItem(uri, String.Empty);

            return null;
        }

        /// <summary>
        ///		Retrieves media items by a search pattern
        /// </summary>
        /// <returns>
        ///		The resultset
        /// </returns>
        public MediaSourceSearchResult Search(string pattern)
        {
            List<MediaItem> items = new List<MediaItem>();

            foreach (string streamUri in streamUris)
                if (streamUri.Contains(pattern))
                    items.Add(new MediaItem(new Uri(streamUri), String.Empty));

            return new MediaSourceSearchResult(Identifier, items);
        }

        #endregion
    }
}
