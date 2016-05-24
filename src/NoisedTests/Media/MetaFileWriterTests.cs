using Moq;
using Noised.Core.Config;
using Noised.Core.etc;
using Noised.Core.Media;
using NUnit.Framework;

namespace NoisedTests.Media
{
    [TestFixture]
    public class MetaFileWriterTests
    {
        [Test]
        public void MetaFileWriterConstructorTest()
        {
            var cfgMock = new Mock<IConfig>();
            cfgMock.Setup(x => x.GetProperty(CoreConfigProperties.MetaFileDirectory, null)).Returns(@"C:\test\");

            var mfw = new MetaFileWriter(cfgMock.Object);

            Assert.IsInstanceOf<MetaFileWriter>(mfw);
        }

        [Test]
        public void WriteMetaFileToDiskTest()
        {
            var cfgMock = new Mock<IConfig>();
            cfgMock.Setup(x => x.GetProperty(CoreConfigProperties.MetaFileDirectory, null)).Returns(@"C:\test\");

            var mfw = new MetaFileWriter(cfgMock.Object);

            var mfMock = new Mock<IMetaFile>();
            mfMock.Setup(x => x.Artist).Returns("MockArtist");
            mfMock.Setup(x => x.Album).Returns("MockAlbum");
            mfMock.Setup(x => x.OriginalFilename).Returns("MockFilename.mck");
            mfMock.Setup(x => x.Data).Returns(new byte[] { 1, 2, 3 });

            mfw.WriteMetaFileToDisk(mfMock.Object);
        }
    }
}
