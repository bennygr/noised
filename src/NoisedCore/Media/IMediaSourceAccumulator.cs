using System;
using System.Collections.Generic;

namespace Noised.Core.Media
{
    /// <summary>
    ///		Provides access to all known IMedaSource 
    /// </summary>
    public interface IMediaSourceAccumulator
    {
        /// <summary>
        /// Refreshs all known MediaSources
        /// </summary>
        void Refresh();

        /// <summary>
        /// Searches all known IMediaSources for the given URI
        /// </summary>
        /// <returns> The first found MediaItem for the given URI, or null if no such item was found </returns>
        MediaItem Get(Uri uri);

        /// <summary>
        /// Searches all known IMediaSources for the given search pattern
        /// </summary>
        /// <param name="pattern">The search pattern</param>
        /// <returns>
        /// An enumeration of search results matching the given pattern
        /// </returns>
        IEnumerable<MediaSourceSearchResult> Search(string pattern);

        /// <summary>
        /// Searches a set of IMediaSources for the given search pattern
        /// </summary>
        /// <param name="pattern">The search pattern</param>
        /// <param name="sourceIdentifiers">The identifiers of the IMediaSources to search in</param>
        /// <returns>
        /// An enumeration of search results matching the given pattern
        /// </returns>
        IEnumerable<MediaSourceSearchResult> Search(string pattern, IEnumerable<string> sourceIdentifiers);

        /// <summary>
        /// Gets all Sources
        /// </summary>
        IEnumerable<string> Sources { get; }
    };
}
