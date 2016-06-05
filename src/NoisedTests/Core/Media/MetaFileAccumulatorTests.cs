using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Noised.Core.DB;
using Noised.Core.Media;
using Noised.Core.Media.NoisedMetaFile;
using Noised.Core.Plugins;
using Noised.Core.Plugins.Media;
using NUnit.Framework;

namespace NoisedTests.Core.Media
{
    [TestFixture]
    public class MetaFileAccumulatorTests
    {
        [Test]
        public void MetaFileAccumulator_Constructor_WithoutPluginLoader_ShouldThrowException()
        {
            try
            {
                var dbFactoryMock = new Mock<IDbFactory>();
                var metaFileWriterMock = new Mock<IMetaFileWriter>();
                var mediaSourceAccumulator = new Mock<IMediaSourceAccumulator>();
                var metaFileCleaner = new Mock<IMetaFileCleaner>();

                new MetaFileAccumulator(null, dbFactoryMock.Object, metaFileWriterMock.Object, mediaSourceAccumulator.Object, metaFileCleaner.Object);
                Assert.Fail("Expected ArgumentNullException.");
            }
            catch (ArgumentNullException e)
            {
                Assert.AreSame("pluginLoader", e.ParamName);
            }
        }

        [Test]
        public void MetaFileAccumulator_Constructor_WithoutDbFactory_ShouldThrowException()
        {
            try
            {
                var pluginLoaderMock = new Mock<IPluginLoader>();
                var metaFileWriterMock = new Mock<IMetaFileWriter>();
                var mediaSourceAccumulator = new Mock<IMediaSourceAccumulator>();
                var metaFileCleaner = new Mock<IMetaFileCleaner>();

                new MetaFileAccumulator(pluginLoaderMock.Object, null, metaFileWriterMock.Object, mediaSourceAccumulator.Object, metaFileCleaner.Object);
                Assert.Fail("Expected ArgumentNullException.");
            }
            catch (ArgumentNullException e)
            {
                Assert.AreSame("dbFactory", e.ParamName);
            }
        }

        [Test]
        public void MetaFileAccumulator_Constructor_WithoutMetaFileWriter_ShouldThrowException()
        {
            try
            {
                var pluginLoaderMock = new Mock<IPluginLoader>();
                var dbFactoryMock = new Mock<IDbFactory>();
                var mediaSourceAccumulator = new Mock<IMediaSourceAccumulator>();
                var metaFileCleaner = new Mock<IMetaFileCleaner>();

                new MetaFileAccumulator(pluginLoaderMock.Object, dbFactoryMock.Object, null, mediaSourceAccumulator.Object, metaFileCleaner.Object);
                Assert.Fail("Expected ArgumentNullException.");
            }
            catch (ArgumentNullException e)
            {
                Assert.AreSame("metaFileWriter", e.ParamName);
            }
        }

        [Test]
        public void MetaFileAccumulator_Constructor_WithoutMediaSourceAccumulator_ShouldThrowException()
        {
            try
            {
                var pluginLoaderMock = new Mock<IPluginLoader>();
                var dbFactoryMock = new Mock<IDbFactory>();
                var metaFileWriterMock = new Mock<IMetaFileWriter>();
                var metaFileCleaner = new Mock<IMetaFileCleaner>();

                new MetaFileAccumulator(pluginLoaderMock.Object, dbFactoryMock.Object, metaFileWriterMock.Object, null, metaFileCleaner.Object);
                Assert.Fail("Expected ArgumentNullException.");
            }
            catch (ArgumentNullException e)
            {
                Assert.AreSame("mediaSourceAccumulator", e.ParamName);
            }
        }

        [Test]
        public void MetaFileAccumulator_Constructor_WithoutMetaFileCleaner_ShouldThrowException()
        {
            try
            {
                var pluginLoaderMock = new Mock<IPluginLoader>();
                var dbFactoryMock = new Mock<IDbFactory>();
                var metaFileWriterMock = new Mock<IMetaFileWriter>();
                var mediaSourceAccumulator = new Mock<IMediaSourceAccumulator>();

                new MetaFileAccumulator(pluginLoaderMock.Object, dbFactoryMock.Object, metaFileWriterMock.Object, mediaSourceAccumulator.Object, null);
                Assert.Fail("Expected ArgumentNullException.");
            }
            catch (ArgumentNullException e)
            {
                Assert.AreSame("metaFileCleaner", e.ParamName);
            }
        }

        [Test]
        public void MetaFileAccumulator_Constructor_AllParametersPresent_CanCreateInstance()
        {
            var pluginLoaderMock = new Mock<IPluginLoader>();
            var dbFactoryMock = new Mock<IDbFactory>();
            var metaFileWriterMock = new Mock<IMetaFileWriter>();
            var mediaSourceAccumulator = new Mock<IMediaSourceAccumulator>();
            var metaFileCleaner = new Mock<IMetaFileCleaner>();

            new MetaFileAccumulator(pluginLoaderMock.Object, dbFactoryMock.Object, metaFileWriterMock.Object,
                mediaSourceAccumulator.Object, metaFileCleaner.Object);
        }

        [Test]
        public void MetaFileAccumulator_Refresh_OneMediaSourceSearchResult_RefreshMetaFiles()
        {
            // Arrange
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

            var metaDataList = new List<MetaData>();
            foreach (var item in testData)
            {
                metaDataList.Add(new MetaData
                {
                    Artists = new List<string> { item.Item1 },
                    Album = item.Item2,
                    AlbumArtists = new List<string>()
                });
            }

            var mediaItemList = new List<MediaItem>();
            foreach (var item in metaDataList)
                mediaItemList.Add(new MediaItem(new Uri("file://"), "checksum") { MetaData = item });

            var searchResult = new MediaSourceSearchResult("mockSource", mediaItemList);

            // Mocks
            var mfScraper = new Mock<IMetaFileScraper>();
            mfScraper.Setup(x => x.GetArtistPictures(It.IsAny<string>()))
                .Returns<string>(
                    x =>
                        new List<MetaFile>
                        {
                            new MetaFile(x, String.Empty, MetaFileType.ArtistPicture, new Uri("file://"), null, "ext",
                                MetaFileCategory.Gallery, "orifilename")
                        });
            mfScraper.Setup(x => x.GetAlbumCover(It.IsAny<ScraperAlbumInformation>()))
                .Returns<ScraperAlbumInformation>(
                    x =>
                        new List<MetaFile>
                        {
                            new MetaFile(x.Artist, x.Album, MetaFileType.ArtistPicture, new Uri("file://"), null, "ext",
                                MetaFileCategory.Gallery, "orifilename")
                        });

            var pL = new Mock<IPluginLoader>();
            pL.Setup(x => x.GetPlugins<IMetaFileScraper>()).Returns(new List<IMetaFileScraper> { mfScraper.Object });

            var uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.MetaFileRepository.CreateMetaFile(It.IsAny<MetaFile>()));
            uow.Setup(x => x.SaveChanges());

            var dbFac = new Mock<IDbFactory>();
            dbFac.Setup(x => x.GetUnitOfWork()).Returns(uow.Object);

            var mfWriter = new Mock<IMetaFileWriter>();
            mfWriter.Setup(x => x.WriteMetaFileToDisk(It.IsAny<IMetaFile>()));

            var msAcc = new Mock<IMediaSourceAccumulator>();
            msAcc.Setup(x => x.Search(It.IsAny<string>())).Returns(new List<MediaSourceSearchResult> { searchResult });

            var metaFileCleaner = new Mock<IMetaFileCleaner>();

            // Act
            var mfa = new MetaFileAccumulator(pL.Object, dbFac.Object, mfWriter.Object, msAcc.Object, metaFileCleaner.Object);
            mfa.Refresh();

            // Assert
            mfScraper.Verify(x => x.GetArtistPictures(It.IsIn(testData.Select(y => y.Item1))), Times.Exactly(9)); // 9 because 9 unique artists
            mfScraper.Verify(
                x =>
                    x.GetAlbumCover(
                        It.Is<ScraperAlbumInformation>(
                            y => testData.Any(z => z.Item1 == y.Artist) && testData.Any(z => z.Item2 == y.Album))),
                Times.Exactly(11)); // 11 because 11 unique albums
            mfWriter.Verify(
                x =>
                    x.WriteMetaFileToDisk(
                        It.Is<MetaFile>(
                            y => testData.Any(z => z.Item1 == y.Artist) && testData.Any(z => z.Item2 == y.Album))),
                Times.AtLeastOnce);
            uow.Verify(x => x.MetaFileRepository.CreateMetaFile(It.IsAny<MetaFile>()), Times.AtLeastOnce);
            uow.Verify(x => x.SaveChanges(), Times.AtLeastOnce);
            metaFileCleaner.Verify(x => x.CleanUpMetaFiles(), Times.AtLeastOnce);
        }

        [Test]
        public void MetaFileAccumulator_Refresh_NoMediaSourceSearchResult_DoNothing()
        {
            // Arrange
            // Mocks
            var mfScraper = new Mock<IMetaFileScraper>();
            mfScraper.Setup(x => x.GetArtistPictures(It.IsAny<string>()))
                .Returns<string>(
                    x =>
                        new List<MetaFile>
                        {
                            new MetaFile(x, String.Empty, MetaFileType.ArtistPicture, new Uri("file://"), null, "ext",
                                MetaFileCategory.Gallery, "orifilename")
                        });
            mfScraper.Setup(x => x.GetAlbumCover(It.IsAny<ScraperAlbumInformation>()))
                .Returns<ScraperAlbumInformation>(
                    x =>
                        new List<MetaFile>
                        {
                            new MetaFile(x.Artist, x.Album, MetaFileType.ArtistPicture, new Uri("file://"), null, "ext",
                                MetaFileCategory.Gallery, "orifilename")
                        });

            var pL = new Mock<IPluginLoader>();
            pL.Setup(x => x.GetPlugins<IMetaFileScraper>()).Returns(new List<IMetaFileScraper> { mfScraper.Object });

            var uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.MetaFileRepository.CreateMetaFile(It.IsAny<MetaFile>()));
            uow.Setup(x => x.SaveChanges());

            var dbFac = new Mock<IDbFactory>();
            dbFac.Setup(x => x.GetUnitOfWork()).Returns(uow.Object);

            var mfWriter = new Mock<IMetaFileWriter>();
            mfWriter.Setup(x => x.WriteMetaFileToDisk(It.IsAny<IMetaFile>()));

            var msAcc = new Mock<IMediaSourceAccumulator>();
            msAcc.Setup(x => x.Search(It.IsAny<string>())).Returns(new List<MediaSourceSearchResult>());

            var metaFileCleaner = new Mock<IMetaFileCleaner>();

            // Act
            var mfa = new MetaFileAccumulator(pL.Object, dbFac.Object, mfWriter.Object, msAcc.Object, metaFileCleaner.Object);
            mfa.Refresh();

            // Assert
            mfScraper.Verify(x => x.GetArtistPictures(It.IsAny<string>()), Times.Never);
            mfScraper.Verify(x => x.GetAlbumCover(It.IsAny<ScraperAlbumInformation>()), Times.Never());
            mfWriter.Verify(x => x.WriteMetaFileToDisk(It.IsAny<MetaFile>()), Times.Never);
            uow.Verify(x => x.MetaFileRepository.CreateMetaFile(It.IsAny<MetaFile>()), Times.Never);
            uow.Verify(x => x.SaveChanges(), Times.Never);
            metaFileCleaner.Verify(x => x.CleanUpMetaFiles(), Times.AtLeastOnce);
        }

        [Test]
        public void MetaFileAccumulator_Refresh_NoMetaFileScraperPlugins_DoNothing()
        {
            // Arrange
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

            var metaDataList = new List<MetaData>();
            foreach (var item in testData)
            {
                metaDataList.Add(new MetaData
                {
                    Artists = new List<string> { item.Item1 },
                    Album = item.Item2,
                    AlbumArtists = new List<string>()
                });
            }

            var mediaItemList = new List<MediaItem>();
            foreach (var item in metaDataList)
                mediaItemList.Add(new MediaItem(new Uri("file://"), "checksum") { MetaData = item });

            var searchResult = new MediaSourceSearchResult("mockSource", mediaItemList);

            // Mocks
            var pL = new Mock<IPluginLoader>();
            pL.Setup(x => x.GetPlugins<IMetaFileScraper>()).Returns(new List<IMetaFileScraper>());

            var uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.MetaFileRepository.CreateMetaFile(It.IsAny<MetaFile>()));
            uow.Setup(x => x.SaveChanges());

            var dbFac = new Mock<IDbFactory>();
            dbFac.Setup(x => x.GetUnitOfWork()).Returns(uow.Object);

            var mfWriter = new Mock<IMetaFileWriter>();
            mfWriter.Setup(x => x.WriteMetaFileToDisk(It.IsAny<IMetaFile>()));

            var msAcc = new Mock<IMediaSourceAccumulator>();
            msAcc.Setup(x => x.Search(It.IsAny<string>())).Returns(new List<MediaSourceSearchResult> { searchResult });

            var metaFileCleaner = new Mock<IMetaFileCleaner>();

            // Act
            var mfa = new MetaFileAccumulator(pL.Object, dbFac.Object, mfWriter.Object, msAcc.Object, metaFileCleaner.Object);
            mfa.Refresh();

            // Assert
            mfWriter.Verify(x => x.WriteMetaFileToDisk(It.IsAny<MetaFile>()), Times.Never);
            uow.Verify(x => x.MetaFileRepository.CreateMetaFile(It.IsAny<MetaFile>()), Times.Never);
            uow.Verify(x => x.SaveChanges(), Times.Never);
            metaFileCleaner.Verify(x => x.CleanUpMetaFiles(), Times.AtLeastOnce);
        }

        [Test]
        public void MetaFileAccumulator_Refresh_NoMetaFiles_DoNotRefreshMetaFiles()
        {
            // Arrange
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

            var metaDataList = new List<MetaData>();
            foreach (var item in testData)
            {
                metaDataList.Add(new MetaData
                {
                    Artists = new List<string> { item.Item1 },
                    Album = item.Item2,
                    AlbumArtists = new List<string>()
                });
            }

            var mediaItemList = new List<MediaItem>();
            foreach (var item in metaDataList)
                mediaItemList.Add(new MediaItem(new Uri("file://"), "checksum") { MetaData = item });

            var searchResult = new MediaSourceSearchResult("mockSource", mediaItemList);

            // Mocks
            var mfScraper = new Mock<IMetaFileScraper>();
            mfScraper.Setup(x => x.GetArtistPictures(It.IsAny<string>())).Returns(new List<MetaFile>());
            mfScraper.Setup(x => x.GetAlbumCover(It.IsAny<ScraperAlbumInformation>())).Returns(new List<MetaFile>());

            var pL = new Mock<IPluginLoader>();
            pL.Setup(x => x.GetPlugins<IMetaFileScraper>()).Returns(new List<IMetaFileScraper> { mfScraper.Object });

            var uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.MetaFileRepository.CreateMetaFile(It.IsAny<MetaFile>()));
            uow.Setup(x => x.SaveChanges());

            var dbFac = new Mock<IDbFactory>();
            dbFac.Setup(x => x.GetUnitOfWork()).Returns(uow.Object);

            var mfWriter = new Mock<IMetaFileWriter>();
            mfWriter.Setup(x => x.WriteMetaFileToDisk(It.IsAny<IMetaFile>()));

            var msAcc = new Mock<IMediaSourceAccumulator>();
            msAcc.Setup(x => x.Search(It.IsAny<string>())).Returns(new List<MediaSourceSearchResult> { searchResult });

            var metaFileCleaner = new Mock<IMetaFileCleaner>();

            // Act
            var mfa = new MetaFileAccumulator(pL.Object, dbFac.Object, mfWriter.Object, msAcc.Object, metaFileCleaner.Object);
            mfa.Refresh();

            // Assert
            mfScraper.Verify(x => x.GetArtistPictures(It.IsIn(testData.Select(y => y.Item1))), Times.Exactly(9)); // 9 because 9 unique artists
            mfScraper.Verify(
                x =>
                    x.GetAlbumCover(
                        It.Is<ScraperAlbumInformation>(
                            y => testData.Any(z => z.Item1 == y.Artist) && testData.Any(z => z.Item2 == y.Album))),
                Times.Exactly(11)); // 11 because 11 unique albums
            mfWriter.Verify(x => x.WriteMetaFileToDisk(It.IsAny<MetaFile>()), Times.Never);
            uow.Verify(x => x.MetaFileRepository.CreateMetaFile(It.IsAny<MetaFile>()), Times.Never);
            uow.Verify(x => x.SaveChanges(), Times.Never);
            metaFileCleaner.Verify(x => x.CleanUpMetaFiles(), Times.AtLeastOnce);
        }

        [Test]
        public async void MetaFileAccumulator_RefreshAsync_OneMediaSourceSearchResult_RefreshMetaFiles()
        {
            // Arrange
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

            var metaDataList = new List<MetaData>();
            foreach (var item in testData)
            {
                metaDataList.Add(new MetaData
                {
                    Artists = new List<string> { item.Item1 },
                    Album = item.Item2,
                    AlbumArtists = new List<string>()
                });
            }

            var mediaItemList = new List<MediaItem>();
            foreach (var item in metaDataList)
                mediaItemList.Add(new MediaItem(new Uri("file://"), "checksum") { MetaData = item });

            var searchResult = new MediaSourceSearchResult("mockSource", mediaItemList);

            // Mocks
            var mfScraper = new Mock<IMetaFileScraper>();
            mfScraper.Setup(x => x.GetArtistPictures(It.IsAny<string>()))
                .Returns<string>(
                    x =>
                        new List<MetaFile>
                        {
                            new MetaFile(x, String.Empty, MetaFileType.ArtistPicture, new Uri("file://"), null, "ext",
                                MetaFileCategory.Gallery, "orifilename")
                        });
            mfScraper.Setup(x => x.GetAlbumCover(It.IsAny<ScraperAlbumInformation>()))
                .Returns<ScraperAlbumInformation>(
                    x =>
                        new List<MetaFile>
                        {
                            new MetaFile(x.Artist, x.Album, MetaFileType.ArtistPicture, new Uri("file://"), null, "ext",
                                MetaFileCategory.Gallery, "orifilename")
                        });

            var pL = new Mock<IPluginLoader>();
            pL.Setup(x => x.GetPlugins<IMetaFileScraper>()).Returns(new List<IMetaFileScraper> { mfScraper.Object });

            var uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.MetaFileRepository.CreateMetaFile(It.IsAny<MetaFile>()));
            uow.Setup(x => x.SaveChanges());

            var dbFac = new Mock<IDbFactory>();
            dbFac.Setup(x => x.GetUnitOfWork()).Returns(uow.Object);

            var mfWriter = new Mock<IMetaFileWriter>();
            mfWriter.Setup(x => x.WriteMetaFileToDisk(It.IsAny<IMetaFile>()));

            var msAcc = new Mock<IMediaSourceAccumulator>();
            msAcc.Setup(x => x.Search(It.IsAny<string>())).Returns(new List<MediaSourceSearchResult> { searchResult });

            var metaFileCleaner = new Mock<IMetaFileCleaner>();

            // Act
            var mfa = new MetaFileAccumulator(pL.Object, dbFac.Object, mfWriter.Object, msAcc.Object, metaFileCleaner.Object);
            await mfa.RefreshAsync();

            // Assert
            mfScraper.Verify(x => x.GetArtistPictures(It.IsIn(testData.Select(y => y.Item1))), Times.Exactly(9)); // 9 because 9 unique artists
            mfScraper.Verify(
                x =>
                    x.GetAlbumCover(
                        It.Is<ScraperAlbumInformation>(
                            y => testData.Any(z => z.Item1 == y.Artist) && testData.Any(z => z.Item2 == y.Album))),
                Times.Exactly(11)); // 11 because 11 unique albums
            mfWriter.Verify(
                x =>
                    x.WriteMetaFileToDisk(
                        It.Is<MetaFile>(
                            y => testData.Any(z => z.Item1 == y.Artist) && testData.Any(z => z.Item2 == y.Album))),
                Times.AtLeastOnce);
            uow.Verify(x => x.MetaFileRepository.CreateMetaFile(It.IsAny<MetaFile>()), Times.AtLeastOnce);
            uow.Verify(x => x.SaveChanges(), Times.AtLeastOnce);
            metaFileCleaner.Verify(x => x.CleanUpMetaFiles(), Times.AtLeastOnce);
        }

        [Test]
        public async void MetaFileAccumulator_RefreshAsync_NoMediaSourceSearchResult_DoNothing()
        {
            // Arrange
            // Mocks
            var mfScraper = new Mock<IMetaFileScraper>();
            mfScraper.Setup(x => x.GetArtistPictures(It.IsAny<string>()))
                .Returns<string>(
                    x =>
                        new List<MetaFile>
                        {
                            new MetaFile(x, String.Empty, MetaFileType.ArtistPicture, new Uri("file://"), null, "ext",
                                MetaFileCategory.Gallery, "orifilename")
                        });
            mfScraper.Setup(x => x.GetAlbumCover(It.IsAny<ScraperAlbumInformation>()))
                .Returns<ScraperAlbumInformation>(
                    x =>
                        new List<MetaFile>
                        {
                            new MetaFile(x.Artist, x.Album, MetaFileType.ArtistPicture, new Uri("file://"), null, "ext",
                                MetaFileCategory.Gallery, "orifilename")
                        });

            var pL = new Mock<IPluginLoader>();
            pL.Setup(x => x.GetPlugins<IMetaFileScraper>()).Returns(new List<IMetaFileScraper> { mfScraper.Object });

            var uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.MetaFileRepository.CreateMetaFile(It.IsAny<MetaFile>()));
            uow.Setup(x => x.SaveChanges());

            var dbFac = new Mock<IDbFactory>();
            dbFac.Setup(x => x.GetUnitOfWork()).Returns(uow.Object);

            var mfWriter = new Mock<IMetaFileWriter>();
            mfWriter.Setup(x => x.WriteMetaFileToDisk(It.IsAny<IMetaFile>()));

            var msAcc = new Mock<IMediaSourceAccumulator>();
            msAcc.Setup(x => x.Search(It.IsAny<string>())).Returns(new List<MediaSourceSearchResult>());

            var metaFileCleaner = new Mock<IMetaFileCleaner>();

            // Act
            var mfa = new MetaFileAccumulator(pL.Object, dbFac.Object, mfWriter.Object, msAcc.Object, metaFileCleaner.Object);
            await mfa.RefreshAsync();

            // Assert
            mfScraper.Verify(x => x.GetArtistPictures(It.IsAny<string>()), Times.Never);
            mfScraper.Verify(x => x.GetAlbumCover(It.IsAny<ScraperAlbumInformation>()), Times.Never());
            mfWriter.Verify(x => x.WriteMetaFileToDisk(It.IsAny<MetaFile>()), Times.Never);
            uow.Verify(x => x.MetaFileRepository.CreateMetaFile(It.IsAny<MetaFile>()), Times.Never);
            uow.Verify(x => x.SaveChanges(), Times.Never);
            metaFileCleaner.Verify(x => x.CleanUpMetaFiles(), Times.AtLeastOnce);
        }

        [Test]
        public async void MetaFileAccumulator_RefreshAsync_NoMetaFileScraperPlugins_DoNothing()
        {
            // Arrange
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

            var metaDataList = new List<MetaData>();
            foreach (var item in testData)
            {
                metaDataList.Add(new MetaData
                {
                    Artists = new List<string> { item.Item1 },
                    Album = item.Item2,
                    AlbumArtists = new List<string>()
                });
            }

            var mediaItemList = new List<MediaItem>();
            foreach (var item in metaDataList)
                mediaItemList.Add(new MediaItem(new Uri("file://"), "checksum") { MetaData = item });

            var searchResult = new MediaSourceSearchResult("mockSource", mediaItemList);

            // Mocks
            var pL = new Mock<IPluginLoader>();
            pL.Setup(x => x.GetPlugins<IMetaFileScraper>()).Returns(new List<IMetaFileScraper>());

            var uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.MetaFileRepository.CreateMetaFile(It.IsAny<MetaFile>()));
            uow.Setup(x => x.SaveChanges());

            var dbFac = new Mock<IDbFactory>();
            dbFac.Setup(x => x.GetUnitOfWork()).Returns(uow.Object);

            var mfWriter = new Mock<IMetaFileWriter>();
            mfWriter.Setup(x => x.WriteMetaFileToDisk(It.IsAny<IMetaFile>()));

            var msAcc = new Mock<IMediaSourceAccumulator>();
            msAcc.Setup(x => x.Search(It.IsAny<string>())).Returns(new List<MediaSourceSearchResult> { searchResult });

            var metaFileCleaner = new Mock<IMetaFileCleaner>();

            // Act
            var mfa = new MetaFileAccumulator(pL.Object, dbFac.Object, mfWriter.Object, msAcc.Object, metaFileCleaner.Object);
            await mfa.RefreshAsync();

            // Assert
            mfWriter.Verify(x => x.WriteMetaFileToDisk(It.IsAny<MetaFile>()), Times.Never);
            uow.Verify(x => x.MetaFileRepository.CreateMetaFile(It.IsAny<MetaFile>()), Times.Never);
            uow.Verify(x => x.SaveChanges(), Times.Never);
            metaFileCleaner.Verify(x => x.CleanUpMetaFiles(), Times.AtLeastOnce);
        }

        [Test]
        public async void MetaFileAccumulator_RefreshAsyn_NoMetaFiles_DoNotRefreshMetaFiles()
        {
            // Arrange
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

            var metaDataList = new List<MetaData>();
            foreach (var item in testData)
            {
                metaDataList.Add(new MetaData
                {
                    Artists = new List<string> { item.Item1 },
                    Album = item.Item2,
                    AlbumArtists = new List<string>()
                });
            }

            var mediaItemList = new List<MediaItem>();
            foreach (var item in metaDataList)
                mediaItemList.Add(new MediaItem(new Uri("file://"), "checksum") { MetaData = item });

            var searchResult = new MediaSourceSearchResult("mockSource", mediaItemList);

            // Mocks
            var mfScraper = new Mock<IMetaFileScraper>();
            mfScraper.Setup(x => x.GetArtistPictures(It.IsAny<string>())).Returns(new List<MetaFile>());
            mfScraper.Setup(x => x.GetAlbumCover(It.IsAny<ScraperAlbumInformation>())).Returns(new List<MetaFile>());

            var pL = new Mock<IPluginLoader>();
            pL.Setup(x => x.GetPlugins<IMetaFileScraper>()).Returns(new List<IMetaFileScraper> { mfScraper.Object });

            var uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.MetaFileRepository.CreateMetaFile(It.IsAny<MetaFile>()));
            uow.Setup(x => x.SaveChanges());

            var dbFac = new Mock<IDbFactory>();
            dbFac.Setup(x => x.GetUnitOfWork()).Returns(uow.Object);

            var mfWriter = new Mock<IMetaFileWriter>();
            mfWriter.Setup(x => x.WriteMetaFileToDisk(It.IsAny<IMetaFile>()));

            var msAcc = new Mock<IMediaSourceAccumulator>();
            msAcc.Setup(x => x.Search(It.IsAny<string>())).Returns(new List<MediaSourceSearchResult> { searchResult });

            var metaFileCleaner = new Mock<IMetaFileCleaner>();

            // Act
            var mfa = new MetaFileAccumulator(pL.Object, dbFac.Object, mfWriter.Object, msAcc.Object, metaFileCleaner.Object);
            await mfa.RefreshAsync();

            // Assert
            mfScraper.Verify(x => x.GetArtistPictures(It.IsIn(testData.Select(y => y.Item1))), Times.Exactly(9)); // 9 because 9 unique artists
            mfScraper.Verify(
                x =>
                    x.GetAlbumCover(
                        It.Is<ScraperAlbumInformation>(
                            y => testData.Any(z => z.Item1 == y.Artist) && testData.Any(z => z.Item2 == y.Album))),
                Times.Exactly(11)); // 11 because 11 unique albums
            mfWriter.Verify(x => x.WriteMetaFileToDisk(It.IsAny<MetaFile>()), Times.Never);
            uow.Verify(x => x.MetaFileRepository.CreateMetaFile(It.IsAny<MetaFile>()), Times.Never);
            uow.Verify(x => x.SaveChanges(), Times.Never);
            metaFileCleaner.Verify(x => x.CleanUpMetaFiles(), Times.AtLeastOnce);
        }
    }
}
