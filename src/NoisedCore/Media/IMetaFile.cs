using System;

namespace Noised.Core.Media
{
    public interface IMetaFile
    {
        /// <summary>
        /// The Artist corresponding to this MetaFile
        /// </summary>
        string Artist { get; }

        /// <summary>
        /// The Album corresponfig to this MetaFile if applicable
        /// </summary>
        string Album { get; }

        /// <summary>
        /// The Uri of the MetaFile
        /// </summary>
        Uri Uri { get; set; }

        /// <summary>
        /// The MetaFile in memory
        /// </summary>
        byte[] Data { get; }

        /// <summary>
        /// The file extension of the MetaFile
        /// </summary>
        string Extension { get; }

        /// <summary>
        /// The Category of the MetaFile
        /// </summary>
        MetaFileCategory Category { get; }

        /// <summary>
        /// The type of the MetaFile
        /// </summary>
        MetaFileType Type { get; }

        /// <summary>
        /// Filename as defined by the source
        /// </summary>
        string OriginalFilename { get; }
    }
}