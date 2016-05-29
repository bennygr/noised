namespace Noised.Core.Media.NoisedMetaFile
{
    /// <summary>
    /// Interface for a class that deletes MetaFileEntries in the database if the correspondig files are no longer present
    /// </summary>
    public interface IMetaFileCleaner
    {
        /// <summary>
        /// Method that deletes MetaFileEntries in the database if the correspondig files are no longer present
        /// </summary>
        void CleanUpMetaFiles();
    }
}
