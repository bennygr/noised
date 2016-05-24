﻿using System;
using System.IO;
using Noised.Core.Config;
using Noised.Core.etc;

namespace Noised.Core.Media
{
    /// <summary>
    /// A class which can write a MetaFile to the designated directory
    /// </summary>
    public class MetaFileWriter : IMetaFileWriter
    {
        private readonly string metaFilePath;

        /// <summary>
        /// A class which can write a MetaFile to the designated directory
        /// </summary>
        public MetaFileWriter(IConfig configuration)
        {
            metaFilePath = configuration.GetProperty(CoreConfigProperties.MetaFileDirectory);
            CreateDirectoryIfNotExists(metaFilePath);
        }

        #region Implementation of IMetaFileWriter

        /// <summary>
        /// The method which writes the MetaFile to the disk
        /// </summary>
        /// <param name="metaFile">The MetaFile which should be written to the disk</param>
        public void WriteMetaFileToDisk(IMetaFile metaFile)
        {
            // always append the artist
            string fullPath = Path.Combine(metaFilePath, metaFile.Artist);

            // if the metafile has an album attach the album
            if (!String.IsNullOrWhiteSpace(metaFile.Album))
                fullPath = Path.Combine(fullPath, metaFile.Album);

            // create the directory if it does not already exist
            CreateDirectoryIfNotExists(fullPath);

            // attach the filename
            fullPath = Path.Combine(fullPath, metaFile.OriginalFilename);

            // write only if file not already exists
            if (File.Exists(fullPath))
                return;

            // change URI to local URI
            metaFile.Uri = new Uri(fullPath);

            // write file to disk
            File.WriteAllBytes(fullPath, metaFile.Data);
        }

        #endregion

        private static void CreateDirectoryIfNotExists(string dir)
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }
    }
}
