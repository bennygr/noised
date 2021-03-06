﻿using System;

namespace Noised.Core.Media.NoisedMetaFile
{
    /// <summary>
    /// A file consisting meta informations about media entities like album covers and artist pictures
    /// </summary>
    public class MetaFile : IMetaFile
    {
        private readonly MetaFileType type;
        private readonly MetaFileCategory category;

        /// <summary>
        /// The Artist corresponding to this MetaFile
        /// </summary>
        public string Artist { get; private set; }

        /// <summary>
        /// The Album corresponfig to this MetaFile if applicable
        /// </summary>
        public string Album { get; private set; }

        /// <summary>
        /// The Uri of the MetaFile
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// The MetaFile in memory
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// The file extension of the MetaFile
        /// </summary>
        public string Extension { get; private set; }

        /// <summary>
        /// The Category of the MetaFile
        /// </summary>
        public MetaFileCategory Category
        {
            get
            {
                return category;
            }
        }

        /// <summary>
        /// The type of the MetaFile
        /// </summary>
        public MetaFileType Type
        {
            get
            {
                return type;
            }
        }

        /// <summary>
        /// Filename as defined by the source
        /// </summary>
        public string OriginalFilename { get; private set; }

        /// <summary>
        /// A file consisting meta informations about media entities like album covers and artist pictures
        /// </summary>
        /// <param name="artist">The Artist corresponding to this MetaFile</param>
        /// <param name="album">The Album corresponfig to this MetaFile if applicable</param>
        /// <param name="type">The type of the MetaFile</param>
        /// <param name="uri">The Uri of the MetaFile</param>
        /// <param name="data">The MetaFile in memory</param>
        /// <param name="extension">The file extension of the MetaFile</param>
        /// <param name="category">The category of the MetaFile</param>
        /// <param name="originalFilename">Filename as defined by the source</param>
        public MetaFile(string artist, string album, string type, Uri uri, byte[] data, string extension, string category, string originalFilename)
        {
            if (String.IsNullOrWhiteSpace(artist))
                throw new ArgumentNullException("artist");
            if (String.IsNullOrWhiteSpace(type))
                throw new ArgumentNullException("type");
            if (uri == null)
                throw new ArgumentNullException("uri");
            if (String.IsNullOrWhiteSpace(originalFilename))
                throw new ArgumentNullException("originalFilename");
            if (String.IsNullOrWhiteSpace(extension))
                throw new ArgumentNullException("extension");
            if (String.IsNullOrWhiteSpace(category))
                throw new ArgumentNullException("category");

            Artist = artist;
            Album = album;
            Uri = uri;
            Data = data;

            if (!Enum.TryParse(type, out this.type))
                throw new ArgumentException("Invalid Type");

            Extension = extension;

            if (!Enum.TryParse(category, out this.category))
                throw new ArgumentException("Invalid Category");

            OriginalFilename = originalFilename;
        }

        /// <summary>
        /// A file consisting meta informations about media entities like album covers and artist pictures
        /// </summary>
        /// <param name="artist">The Artist corresponding to this MetaFile</param>
        /// <param name="album">The Album corresponfig to this MetaFile if applicable</param>
        /// <param name="type">The type of the MetaFile</param>
        /// <param name="uri">The Uri of the MetaFile</param>
        /// <param name="data">The MetaFile in memory</param>
        /// <param name="extension">The file extension of the MetaFile</param>
        /// <param name="category">The category of the MetaFile</param>
        /// <param name="originalFilename">Filename as defined by the source</param>
        public MetaFile(string artist, string album, MetaFileType type, Uri uri, byte[] data, string extension, MetaFileCategory category, string originalFilename)
            : this(artist, album, type.ToString(), uri, data, extension, category.ToString(), originalFilename)
        { }
    }
}
