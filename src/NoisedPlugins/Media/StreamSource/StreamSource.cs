using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Noised.Core.Config;
using Noised.Core.Media;
using Noised.Core.Plugins;
using Noised.Core.Plugins.Media;

namespace Noised.Plugins.Media.StreamSource
{
    /// <summary>
    /// Class to store Streams as a source for MediaItems
    /// </summary>
    public class StreamSource : IMediaSource
    {
        private readonly PluginInitializer initalizer;
        private readonly List<string> streamUris;

        public StreamSource(PluginInitializer initalizer)
        {
            if (initalizer == null)
                throw new ArgumentNullException("initalizer");

            this.initalizer = initalizer;
            streamUris = new List<string>();
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
        {
            string configValue = initalizer.Get<IConfig>().GetProperty("noised.plugins.media.filesystemsource.streams");

            if (String.IsNullOrWhiteSpace(configValue))
            {
                initalizer.Logging.Debug("No Streamsources found. Skipping refresh of streams.");
                return;
            }

            string[] files = configValue.Split(',');

            StringBuilder streamString = new StringBuilder();
            foreach (string file in files)
            {
                initalizer.Logging.Debug("Refreshing from file \"" + file + "\"");

                if (!File.Exists(file))
                {
                    initalizer.Logging.Debug("File does not exist! Skipping file.");
                    continue;
                }

                streamString.Append(File.ReadAllText(file));
            }

            foreach (string singleStreamString in streamString.ToString().Split(','))
                streamUris.Add(singleStreamString);

            initalizer.Logging.Debug(streamUris.Count + " stream(s) added");
        }

        /// <summary>
        ///		Gets the media item for the given URI
        /// </summary>
        /// <param name="uri">The uri</param>
        /// <returns>The MediaItem for the given Uri, or null if no such MediaItem was found</returns> 
        public MediaItem Get(Uri uri)
        {
            // If we have a Uri pointing to the desired stream we create a new MediaItem with it.
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
            // Searching in the List of Uris
            List<MediaItem> items = new List<MediaItem>();

            foreach (string streamUri in streamUris)
                if (streamUri.Contains(pattern))
                    items.Add(new MediaItem(new Uri(streamUri), String.Empty));

            return new MediaSourceSearchResult(Identifier, items);
        }

        #endregion
    }
}
