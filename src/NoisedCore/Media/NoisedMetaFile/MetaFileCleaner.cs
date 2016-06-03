using System;
using Noised.Core.DB;

namespace Noised.Core.Media.NoisedMetaFile
{
    public class MetaFileCleaner : IMetaFileCleaner
    {
        private readonly IDbFactory dbFactory;
        private readonly IMetaFileCleanerFileAccess fileAccess;

        public MetaFileCleaner(IDbFactory dbFactory, IMetaFileCleanerFileAccess fileAccess)
        {
            if (dbFactory == null)
                throw new ArgumentNullException("dbFactory");
            if (fileAccess == null)
                throw new ArgumentNullException("fileAccess");

            this.dbFactory = dbFactory;
            this.fileAccess = fileAccess;
        }

        #region Implementation of IMetaFileCleaner

        /// <summary>
        /// Method that deletes MetaFileEntries in the database if the correspondig files are no longer present
        /// </summary>
        public void CleanUpMetaFiles()
        {
            using (var uow = dbFactory.GetUnitOfWork())
            {
                var metaFileRepository = uow.MetaFileRepository;

                foreach (var metaFile in metaFileRepository.GetAllMetaFiles())
                {
                    if (!fileAccess.FileExists(metaFile.Uri.AbsolutePath))
                        metaFileRepository.DeleteMetaFile(metaFile);
                }

                uow.SaveChanges();
            }
        }

        #endregion
    }
}
