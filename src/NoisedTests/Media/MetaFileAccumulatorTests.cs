using System;
using System.Collections.Generic;
using System.Linq;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Generators;
using Moq;
using Noised.Core.DB;
using Noised.Core.Media;
using Noised.Core.Media.NoisedMetaFile;
using Noised.Core.Plugins;
using Noised.Core.Plugins.Media;
using NUnit.Framework;

namespace NoisedTests.Media
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

                new MetaFileAccumulator(null, dbFactoryMock.Object, metaFileWriterMock.Object, mediaSourceAccumulator.Object);
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

                new MetaFileAccumulator(pluginLoaderMock.Object, null, metaFileWriterMock.Object, mediaSourceAccumulator.Object);
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

                new MetaFileAccumulator(pluginLoaderMock.Object, dbFactoryMock.Object, null, mediaSourceAccumulator.Object);
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

                new MetaFileAccumulator(pluginLoaderMock.Object, dbFactoryMock.Object, metaFileWriterMock.Object, null);
                Assert.Fail("Expected ArgumentNullException.");
            }
            catch (ArgumentNullException e)
            {
                Assert.AreSame("mediaSourceAccumulator", e.ParamName);
            }
        }

        [Test]
        public void MetaFileAccumulator_Constructor_AllParametersPresent_CanCreateInstance()
        {
            var pluginLoaderMock = new Mock<IPluginLoader>();
            var dbFactoryMock = new Mock<IDbFactory>();
            var metaFileWriterMock = new Mock<IMetaFileWriter>();
            var mediaSourceAccumulator = new Mock<IMediaSourceAccumulator>();

            new MetaFileAccumulator(pluginLoaderMock.Object, dbFactoryMock.Object, metaFileWriterMock.Object,
                mediaSourceAccumulator.Object);
        }

        [Test]
        public void MetaFileAccumulator_Refresh_OneMediaSourceSearchResult_RefreshMetaFiles()
        {
            var mfScraper = new Mock<IMetaFileScraper>();
            mfScraper.Setup(x => x.GetArtistPictures(It.IsAny<string>())).Returns(new List<MetaFile> { new MetaFile() });

            var pL = new Mock<IPluginLoader>();
            pL.Setup(x => x.GetPlugins<IMetaFileScraper>()).Returns(new List<IMetaFileScraper> { mfScraper.Object });

            var dbFac = new Mock<IDbFactory>();
            var mfWriter = new Mock<IMetaFileWriter>();

            var msAcc = new Mock<IMediaSourceAccumulator>();
            var metaDataList =
                Builder<MetaData>.CreateListOfSize(10)
                    .All()
                    .With(x => x.AlbumArtists = Enumerable.Range(0, 1).Select(el => GetRandom.String(10)).ToList())
                    .And(x => x.Artists = Enumerable.Range(0, 1).Select(el => GetRandom.String(10)).ToList())
                    .Build();
            var mediaItemList =
                Builder<MediaItem>.CreateListOfSize(10)
                    .All().WithConstructor(() => new MediaItem(new Uri("file://"), "checksum"))
                    .With(x => x.MetaData = Pick<MetaData>.RandomItemFrom(metaDataList))
                    .Build();
            var searchResult =
                Builder<MediaSourceSearchResult>.CreateNew()
                    .WithConstructor(() => new MediaSourceSearchResult("mockSource", mediaItemList)).Build();
            msAcc.Setup(x => x.Search(It.IsAny<string>())).Returns(new List<MediaSourceSearchResult> { searchResult });

            var mfa = new MetaFileAccumulator(pL.Object, dbFac.Object, mfWriter.Object, msAcc.Object);
            mfa.Refresh();
        }

        [Test]
        public void MetaFileAccumulator_Refresh_NoMediaAvailable_DoNotRefreshMetaFiles()
        {
            Assert.Inconclusive();
        }
    }
}
