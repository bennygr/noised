using System.IO;

namespace Noised.Core.Media.NoisedMetaFile
{
    internal class MetaFileIOHandler : IMetaFileIOHandler
    {
        #region Implementation of IMetaFileIOHandler

        public bool MetaFileExists(string fullPath)
        {
            return File.Exists(fullPath);
        }

        public void WriteMetaFileToDisk(string fullPath, byte[] data)
        {
            File.WriteAllBytes(fullPath, data);
        }

        #endregion
    }
}
