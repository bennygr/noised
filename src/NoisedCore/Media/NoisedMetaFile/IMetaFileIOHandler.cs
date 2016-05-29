namespace Noised.Core.Media.NoisedMetaFile
{
    public interface IMetaFileIOHandler
    {
        bool MetaFileExists(string fullPath);

        void WriteMetaFileToDisk(string fullPath, byte[] data);
    }
}
