using System.IO;
using Noised.Core.DB;

namespace Noised.Core.Media.NoisedMetaFile
{
    internal class MetaFileCleaner : IMetaFileCleaner
    {
        private readonly IUnitOfWork unitOfWork;

        public MetaFileCleaner(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #region Implementation of IMetaFileCleaner

        /// <summary>
        /// Method that deletes MetaFileEntries in the database if the correspondig files are no longer present
        /// </summary>
        public void CleanUpMetaFiles()
        {
            using (var repo = unitOfWork)
            {
                var metaFileRepository = repo.MetaFileRepository;

                foreach (var metaFile in metaFileRepository.GetAllMetaFiles())
                {
                    if (!File.Exists(metaFile.Uri.AbsolutePath))
                        metaFileRepository.DeleteMetaFile(metaFile);
                }

                repo.SaveChanges();
            }
        }

        #endregion
    }
}
