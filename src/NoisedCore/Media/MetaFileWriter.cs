using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Noised.Core.Config;
using Noised.Core.IOC;

namespace Noised.Core.Media
{
    public class MetaFileWriter : IMetaFileWriter
    {
        private readonly string metaFilePath;

        public MetaFileWriter()
        {
            metaFilePath = IocContainer.Get<IConfig>().GetProperty("noised.core.metafiles");
        }

        #region Implementation of IMetaFileWriter

        public void WriteMetaFileToDisk(MetaFile metaFile)
        {
            string filename = GetFileName(metaFile);

            string fullPath = Path.Combine(metaFilePath, filename);

            metaFile.Uri = new Uri(fullPath);
            File.WriteAllBytes(fullPath, metaFile.Data);
        }

        #endregion

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
