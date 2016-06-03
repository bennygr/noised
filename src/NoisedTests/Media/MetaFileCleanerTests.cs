using System;
using System.Collections.Generic;
using Moq;
using Noised.Core.DB;
using Noised.Core.Media.NoisedMetaFile;
using NUnit.Framework;

namespace NoisedTests.Media
{
    [TestFixture]
    public class MetaFileCleanerTests
    {
        [Test]
        public void MetaFileCleaner_Constructor_WithoutUnitOfWork_ShouldThrowArgumentNullException()
        {
            try
            {
                var fileAccess = new Mock<IMetaFileIOHandler>();
                new MetaFileCleaner(null, fileAccess.Object);
                Assert.Fail("Should have thrown ArgumentNullException");
            }
            catch (ArgumentNullException ex)
            {
                StringAssert.AreEqualIgnoringCase("dbFactory", ex.ParamName);
            }
        }

        [Test]
        public void MetaFileCleaner_Constructor_WithoutMetaFileCleanerFileAccess_ShouldThrowArgumentNullException()
        {
            try
            {
                var uowMock = new Mock<IDbFactory>();
                new MetaFileCleaner(uowMock.Object, null);
                Assert.Fail("Should have thrown ArgumentNullException");
            }
            catch (ArgumentNullException ex)
            {
                StringAssert.AreEqualIgnoringCase("fileAccess", ex.ParamName);
            }
        }

        [Test]
        public void MetaFileCleaner_Constructor_AllParametersPresent_CanCreateInstance()
        {
            var uowMock = new Mock<IDbFactory>();
            var fileAccess = new Mock<IMetaFileIOHandler>();
            new MetaFileCleaner(uowMock.Object, fileAccess.Object);
        }

        [Test]
        public void MetaFileCleaner_CleanUpMetaFiles_NoMetaFilesInRepository_DoNothing()
        {
            var metaFileRepo = new Mock<IMetaFileRepository>();

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x => x.MetaFileRepository).Returns(metaFileRepo.Object);

            var dbfacMock = new Mock<IDbFactory>();
            dbfacMock.Setup(x => x.GetUnitOfWork()).Returns(uowMock.Object);

            var fileAccess = new Mock<IMetaFileIOHandler>();

            var cleaner = new MetaFileCleaner(dbfacMock.Object, fileAccess.Object);
            cleaner.CleanUpMetaFiles();

            dbfacMock.Verify(x => x.GetUnitOfWork(), Times.Once);
            uowMock.Verify(x => x.SaveChanges(), Times.Once);
            metaFileRepo.Verify(x => x.GetAllMetaFiles(), Times.Once);
            fileAccess.Verify(x => x.MetaFileExists(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void MetaFileCleaner_CleanUpMetaFiles_SeveralMetaFilesInRepository_CleanUp()
        {
            var testData = new List<Tuple<string, string>>
            {
                Tuple.Create("Metallica", "Master of Puppets"),
                Tuple.Create("Metallica", "Death Magnetic"),
                Tuple.Create("SuperAwesomeSuperBand", "SuperAwesomeSuperAlbum"),
                Tuple.Create("System of a Down", "Toxicity"),
                Tuple.Create("Motörhead", "Bomber"),
                Tuple.Create("Avenged Sevenfold", "Hail to the King"),
                Tuple.Create("Foo Fighters", "In your Honor"),
                Tuple.Create("Led Zeppelin", "Led zeppelin IV"),
                Tuple.Create("Lillasyster", "Hjärndöd Kärlek"),
                Tuple.Create("Metallica", "...And justice for all"),
                Tuple.Create("Sabaton", "Heroes")
            };

            var mDataList = new List<IMetaFile>();
            foreach (var item in testData)
                mDataList.Add(new MetaFile(item.Item1, item.Item2, MetaFileType.ArtistPicture, new Uri("file://"), null,
                    "ext", MetaFileCategory.Gallery, "orifilename"));

            var metaFileRepo = new Mock<IMetaFileRepository>();
            metaFileRepo.Setup(x => x.GetAllMetaFiles()).Returns(mDataList);

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x => x.MetaFileRepository).Returns(metaFileRepo.Object);

            var dbfacMock = new Mock<IDbFactory>();
            dbfacMock.Setup(x => x.GetUnitOfWork()).Returns(uowMock.Object);

            var fileAccess = new Mock<IMetaFileIOHandler>();

            var cleaner = new MetaFileCleaner(dbfacMock.Object, fileAccess.Object);
            cleaner.CleanUpMetaFiles();

            dbfacMock.Verify(x => x.GetUnitOfWork(), Times.Once);
            uowMock.Verify(x => x.SaveChanges(), Times.Once);
            metaFileRepo.Verify(x => x.GetAllMetaFiles(), Times.Once);
            fileAccess.Verify(x => x.MetaFileExists(It.IsAny<string>()), Times.Exactly(11));
        }
    }
}
