using Noised.Core.Media;

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
        /// <param name="metaFile"></param>
        void CreateMetaFile(MetaFile metaFile);
    }
}
