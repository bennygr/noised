﻿using System.Collections.Generic;
using Noised.Core.Media;

namespace Noised.Core.Plugins.Media
{
    /// <summary>
    /// Interface for a plugin that scrapes certain sources for MetaFiles
    /// </summary>
    interface IMetaFileScraper : IPlugin
    {
        /// <summary>
        /// Unique Identifier of the source
        /// </summary>
        string Identifier { get; }

        /// <summary>
        /// Method that gets the album cover for an album name
        /// </summary>
        /// <param name="albumName">Name of the album</param>
        /// <returns>One or more album covers</returns>
        IEnumerable<MetaFile> GetAlbumCover(string albumName);

        /// <summary>
        /// Method that gets pictues of an artist
        /// </summary>
        /// <param name="artistName">The name of the artist</param>
        /// <returns>One or more pictues of an artist</returns>
        IEnumerable<MetaFile> GetArtistPictures(string artistName);
    }
}
