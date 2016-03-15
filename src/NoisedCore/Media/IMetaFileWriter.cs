namespace Noised.Core.Media
{
    /// <summary>
    /// Interface for a way to write a MetaFile to the disk
    /// </summary>
    public interface IMetaFileWriter
    {
        /// <summary>
        /// The method which writes the MetaFile to the disk
        /// </summary>
        /// <param name="metaFile">The MetaFile which should be written to the disk</param>
        void WriteMetaFileToDisk(MetaFile metaFile);
    }
}
