using System.IO;
using Noised.Core.DB;
using Noised.Core.IOC;

namespace Noised.Core.Media.NoisedMetaFile
{
    internal class MetaFileCleaner : IMetaFileCleaner
    {
        #region Implementation of IMetaFileCleaner

        /// <summary>
        /// Method that deletes MetaFileEntries in the database if the correspondig files are no longer present
        /// </summary>
        public void CleanUpMetaFiles()
        {
            var metaFileRepository = IoC.Get<IUnitOfWork>().MetaFileRepository;

            foreach (var metaFile in metaFileRepository.GetAllMetaFiles())
            {
                if (!File.Exists(metaFile.Uri.AbsolutePath))
                    metaFileRepository.DeleteMetaFile(metaFile);
            }
        }

        #endregion
    }
}
