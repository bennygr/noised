using System;

namespace Noised.Core.Plugins.Media
{
    public class ScraperAlbumInformation
    {
        public string Artist { get; private set; }
        public string Album { get; private set; }

        public ScraperAlbumInformation(string artist, string album)
        {
            if (String.IsNullOrWhiteSpace(artist))
                throw new ArgumentNullException("artist");
            if (String.IsNullOrWhiteSpace(album))
                throw new ArgumentNullException("album");

            Artist = artist;
            Album = album;
        }
    }
}
