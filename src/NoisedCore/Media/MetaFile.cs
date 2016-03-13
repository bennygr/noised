using System;

namespace Noised.Core.Media
{
    public class MetaFile
    {
        private readonly MetaFileType type;

        public string Artist { get; private set; }
        public string Album { get; private set; }
        public Uri Uri { get; set; }
        public byte[] Data { get; private set; }

        public MetaFileType Type
        {
            get
            {
                return type;
            }
        }

        public MetaFile(string artist, string album, string type, Uri uri, byte[] data)
        {
            if (String.IsNullOrWhiteSpace(artist))
                throw new ArgumentNullException("artist");
            if (String.IsNullOrWhiteSpace(album))
                throw new ArgumentNullException("album");
            if (String.IsNullOrWhiteSpace(type))
                throw new ArgumentNullException("type");
            if (uri == null)
                throw new ArgumentNullException("uri");
            if (data == null)
                throw new ArgumentNullException("data");

            Artist = artist;
            Album = album;
            Uri = uri;
            Data = data;

            if (!Enum.TryParse(type, out this.type))
                throw new ArgumentException("Invalid Type");
        }
    }
}
