using System;
using System.IO;
using Moq;
using Noised.Core.Config;
using Noised.Core.etc;
using Noised.Core.Media.NoisedMetaFile;
using NUnit.Framework;

namespace NoisedTests.Media
{
    [TestFixture]
    public class MetaFileWriterTests
    {
        [Test]
        public void MetaFileWriter_Constructor_AllParametersAvailable_CanCreateInstance()
        {
            var cfgMock = new Mock<IConfig>();
            var ioMock = new Mock<IMetaFileIOHandler>();

            var result = new MetaFileWriter(cfgMock.Object, ioMock.Object);

            Assert.IsNotNull(result);
        }

        [Test]
        public void MetaFileWriter_Constructor_WithoutIConfig_ShouldThrowException()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void MetaFileWriter_Constructor_WithoutIOHandler_ShouldThrowException()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void MetaFileWriter_WriteMetaFileToDiskFileAlreadyExists_MethodNotCalled()
        {
            var cfgMock = new Mock<IConfig>(); 
            string basePathc = Path.Combine("C:\\", "test");
            cfgMock.Setup(x => x.GetProperty(CoreConfigProperties.MetaFileDirectory, null)).Returns(basePathc);

            var ioMock = new Mock<IMetaFileIOHandler>();
            ioMock.Setup(x => x.MetaFileExists(It.IsAny<string>())).Returns(true);
            var callsWriteMethod = false;
            ioMock.Setup(x => x.WriteMetaFileToDisk(It.IsAny<string>(), It.IsAny<byte[]>())).Callback(() =>
            {
                callsWriteMethod = true;
            });

            var mfw = new MetaFileWriter(cfgMock.Object, ioMock.Object);

            var mfMock = new Mock<IMetaFile>();
            mfMock.Setup(x => x.Artist).Returns("MockArtist");
            mfMock.Setup(x => x.Album).Returns("MockAlbum");
            mfMock.Setup(x => x.OriginalFilename).Returns("MockFilename.mck");
            mfMock.Setup(x => x.Data).Returns(new byte[] { 1, 2, 3 });

            mfw.WriteMetaFileToDisk(mfMock.Object);

            Assert.IsFalse(callsWriteMethod);
        }

        [Test]
        public void MetaFileWriter_WriteMetaFileToDisk_Successful()
        {
            var cfgMock = new Mock<IConfig>();
            string basePathc = Path.Combine("C:\\", "test");
            cfgMock.Setup(x => x.GetProperty(CoreConfigProperties.MetaFileDirectory, null)).Returns(basePathc);

            var ioMock = new Mock<IMetaFileIOHandler>();
            ioMock.Setup(x => x.MetaFileExists(It.IsAny<string>())).Returns(false);
            var callsWriteMethod = false;
            var fullPath = String.Empty;
            var data = new byte[0];
            ioMock.Setup(x => x.WriteMetaFileToDisk(It.IsAny<string>(), It.IsAny<byte[]>())).Callback<string, byte[]>((s, b) =>
            {
                fullPath = s;
                data = b;
                callsWriteMethod = true;
            });

            var mfw = new MetaFileWriter(cfgMock.Object, ioMock.Object);

            var artist = "MockArtist";
            var album = "MockAlbum";
            var filename = "MockFilename.mck";

            var mfMock = new Mock<IMetaFile>();

            mfMock.Setup(x => x.Artist).Returns(artist);
            mfMock.Setup(x => x.Album).Returns(album);
            mfMock.Setup(x => x.OriginalFilename).Returns(filename);
            var mockData = new byte[] { 1, 2, 3 };
            mfMock.Setup(x => x.Data).Returns(mockData);

            mfw.WriteMetaFileToDisk(mfMock.Object);

            Assert.IsTrue(callsWriteMethod);
            StringAssert.AreEqualIgnoringCase(Path.Combine(basePathc, artist, album, filename), fullPath);
            Assert.AreSame(mockData, data);
        }
    }
}
