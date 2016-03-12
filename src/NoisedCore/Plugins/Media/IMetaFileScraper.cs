using System.Collections.Generic;
using Noised.Core.Media;

namespace Noised.Core.Plugins.Media
{
    interface IMetaFileScraper : IPlugin
    {
        string Identifier { get; }

        IEnumerable<MetaFile> GetAlbumCover(string albumName);

        IEnumerable<MetaFile> GetArtistPictures(string artistName);
    }
}
