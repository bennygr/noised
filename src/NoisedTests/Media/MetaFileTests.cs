using System;
using Noised.Core.Media.NoisedMetaFile;
using NUnit.Framework;

namespace NoisedTests.Media
{
    [TestFixture]
    public class MetaFileTests
    {
        [Test]
        public void Constructor_Without_Artist()
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
        public void Constructor_Without_Type()
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
        public void Constructor_Without_Uri()
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
        public void Constructor_Without_OriginalFilename()
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
        public void Constructor_Without_Extension()
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
        public void Constructor_Without_Category()
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
    }
}
