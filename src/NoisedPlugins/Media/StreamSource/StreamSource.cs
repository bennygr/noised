using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Noised.Core.Config;
using Noised.Core.Media;
using Noised.Core.Plugins;
using Noised.Core.Plugins.Media;
using YamlDotNet.RepresentationModel;

namespace Noised.Plugins.Media.StreamSource
{
    /// <summary>
    /// Class to store Streams as a source for MediaItems
    /// </summary>
    public class StreamSource : IMediaSource
    {
        private readonly PluginInitializer initalizer;
        private readonly List<MediaItem> streams;

        public StreamSource(PluginInitializer initalizer)
        {
            if (initalizer == null)
                throw new ArgumentNullException("initalizer");

            this.initalizer = initalizer;
            streams = new List<MediaItem>();
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
            // Get configured StreamSource-Files
            string configValue = initalizer.DIContainer.Get<IConfig>().GetProperty("noised.plugins.media.filesystemsource.streams");

            if (String.IsNullOrWhiteSpace(configValue))
            {
                initalizer.Logging.Debug("No Streamsources found. Skipping refresh of streams.");
                return;
            }

            // Iterate over every configured StreamSource-File
            string[] files = configValue.Split(',');
            foreach (string file in files)
            {
                initalizer.Logging.Debug("Refreshing from file \"" + file + "\"");

                if (!File.Exists(file))
                {
                    initalizer.Logging.Debug("File does not exist! Skipping file.");
                    continue;
                }

                // Read streams from file and add to list
                YamlStream yaml = new YamlStream();
                yaml.Load(new StringReader(File.ReadAllText(file)));
                YamlMappingNode mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
                YamlSequenceNode sequence = (YamlSequenceNode)mapping.Children[new YamlScalarNode("streams")];
                foreach (YamlNode yamlNode in sequence)
                {
                    YamlMappingNode stream = (YamlMappingNode)yamlNode;
                    streams.Add(new MediaItem(new Uri(stream.Children[new YamlScalarNode("uri")].ToString()),
                        String.Empty)
                    {
                        MetaData = new MetaData { Title = stream.Children[new YamlScalarNode("title")].ToString() }
                    });
                }
            }

            initalizer.Logging.Debug(streams.Count + " stream(s) added");
        }

        /// <summary>
        ///		Gets the media item for the given URI
        /// </summary>
        /// <param name="uri">The uri</param>
        /// <returns>The MediaItem for the given Uri, or null if no such MediaItem was found</returns> 
        public MediaItem Get(Uri uri)
        {
            // If we have a Uri pointing to the desired stream we create a new MediaItem with it.
            if (streams.Any(x => x.Uri.AbsoluteUri == uri.AbsoluteUri))
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

            foreach (MediaItem item in streams)
            {
                if (item.Uri.AbsoluteUri.Contains(pattern) || item.MetaData.Title.Contains(pattern))
                    items.Add(item);
            }

            return new MediaSourceSearchResult(Identifier, items);
        }

        #endregion
    }
}
