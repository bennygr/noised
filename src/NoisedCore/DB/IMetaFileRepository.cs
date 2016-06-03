using System.Collections.Generic;
using Noised.Core.Media.NoisedMetaFile;

namespace Noised.Core.DB
{
    /// <summary>
    /// Interface of a MetaFileRepository
    /// </summary>
    public interface IMetaFileRepository
    {
        /// <summary>
        /// Creates a new entry for a MetaFile in the current IUnitOfWork implementation
        /// </summary>
        /// <param name="metaFile">MEtaFile to create</param>
        void CreateMetaFile(IMetaFile metaFile);

        /// <summary>
        /// Gets all MetaFiles for the given artist and album
        /// </summary>
        /// <param name="artist">artist for which the metafiles should be searched</param>
        /// <param name="album">album for which the metafiles should be searched</param>
        /// <returns></returns>
        IEnumerable<MetaFile> GetMetaFiles(string artist, string album);

        /// <summary>
        /// Permanently deletes a MetaFile
        /// </summary>
        /// <param name="mf">MetaFile to delete</param>
        void DeleteMetaFile(IMetaFile mf);

        /// <summary>
        /// Gets all MetaFiles
        /// </summary>
        /// <returns></returns>
        IEnumerable<IMetaFile> GetAllMetaFiles();
    }
}
