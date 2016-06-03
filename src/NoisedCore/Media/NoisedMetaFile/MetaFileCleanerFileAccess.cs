using System.IO;

namespace Noised.Core.Media.NoisedMetaFile
{
    public class MetaFileCleanerFileAccess : IMetaFileCleanerFileAccess
    {
        #region Implementation of IMetaFileCleanerFileAccess

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        #endregion
    }
}
