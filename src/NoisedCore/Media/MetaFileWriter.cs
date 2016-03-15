using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Noised.Core.Config;
using Noised.Core.IOC;

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
        public MetaFileWriter()
        {
            metaFilePath = IocContainer.Get<IConfig>().GetProperty("noised.core.metafiles");
        }

        #region Implementation of IMetaFileWriter

        /// <summary>
        /// The method which writes the MetaFile to the disk
        /// </summary>
        /// <param name="metaFile">The MetaFile which should be written to the disk</param>
        public void WriteMetaFileToDisk(MetaFile metaFile)
        {
            string filename = GetFileName(metaFile);

            string fullPath = Path.Combine(metaFilePath, filename);

            metaFile.Uri = new Uri(fullPath);
            File.WriteAllBytes(fullPath, metaFile.Data);
        }

        #endregion

        /// <summary>
        /// Gets a filename and path for a MetaFile
        /// </summary>
        /// <param name="metaFile">The MetaFile for which a path and filename should be created</param>
        /// <returns>A valid pathh and filename</returns>
        private string GetFileName(MetaFile metaFile)
        {
            string path = Path.Combine(metaFilePath, metaFile.Artist, metaFile.Album);

            List<string> files = Directory.EnumerateFileSystemEntries(path, metaFile.Type + "_*").ToList();

            int i = 0;
            string fileName;

            do
            {
                fileName = metaFile.Type + i + metaFile.Extension;
                i++;
            }
            while (files.Contains(fileName));

            return fileName;
        }
    }
}
