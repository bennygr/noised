using System;
using Noised.Core.Media.NoisedMetaFile;
using NUnit.Framework;

namespace NoisedTests.Media
{
    [TestFixture]
    public class MetaFileTests
    {
        [Test]
        public void MetaFile_ConstructorWithoutArtist_ShouldThrowException()
        {
            try
            {
                new MetaFile(String.Empty, String.Empty, MetaFileType.AlbumCover, new Uri("http://www.test.com"), null, "ext",
                    MetaFileCategory.Gallery, "oriFileName");

                Assert.Fail("String.Empty as artist should result in an Exception.");
            }
            catch (ArgumentNullException e)
            {
                Assert.AreSame("artist", e.ParamName);
            }
        }

        [Test]
        public void MetaFile_ConstructorWithoutType_ShouldThrowException()
        {
            try
            {
                new MetaFile("Artist", String.Empty, String.Empty, new Uri("http://www.test.com"), null, "ext",
                    "Gallery", "oriFileName");

                Assert.Fail("String.Empty as type should result in an Exception.");
            }
            catch (ArgumentNullException e)
            {
                Assert.AreSame("type", e.ParamName);
            }
        }

        [Test]
        public void MetaFile_ConstructorWithoutUri_ShouldThrowException()
        {
            try
            {
                new MetaFile("Artist", String.Empty, MetaFileType.AlbumCover, null, null, "ext",
                    MetaFileCategory.Gallery, "oriFileName");

                Assert.Fail("null as uri should result in Exception.");
            }
            catch (ArgumentNullException e)
            {
                Assert.AreSame("uri", e.ParamName);
            }
        }

        [Test]
        public void MetaFile_ConstructorWithoutOriginalFilename_ShouldThrowException()
        {
            try
            {
                new MetaFile("Artist", String.Empty, MetaFileType.AlbumCover, new Uri("http://www.test.com"), null, "ext",
                    MetaFileCategory.Gallery, String.Empty);

                Assert.Fail("String.Empty as originalFilename should result in Exception.");
            }
            catch (ArgumentNullException e)
            {
                Assert.AreSame("originalFilename", e.ParamName);
            }
        }

        [Test]
        public void MetaFile_ConstructorWithoutExtension_ShouldThrowException()
        {
            try
            {
                new MetaFile("Artist", String.Empty, MetaFileType.AlbumCover, new Uri("http://www.test.com"), null, String.Empty,
                    MetaFileCategory.Gallery, "oriFilename");

                Assert.Fail("String.Empty as extension should result in Exception.");
            }
            catch (ArgumentNullException e)
            {
                Assert.AreSame("extension", e.ParamName);
            }
        }

        [Test]
        public void MetaFile_ConstructorWithoutCategory_ShouldThrowException()
        {
            try
            {
                new MetaFile("Artist", String.Empty, "AlbumCover", new Uri("http://www.test.com"), null, "ext",
                    String.Empty, "oriFilename");

                Assert.Fail("String.Empty as category should result in Exception.");
            }
            catch (ArgumentNullException e)
            {
                Assert.AreSame("category", e.ParamName);
            }
        }

        [Test]
        public void MetaFile_Constructor_CanCreateInstance()
        {
            var metaFile = new MetaFile("Artist", "Album", MetaFileType.AlbumCover, new Uri("http://www.uri.com"), null, "ext", MetaFileCategory.Gallery, "oriFileName");

            Assert.IsNotNull(metaFile);
        }
    }
}
