using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Noised.Core.DB;
using Noised.Core.Media;
using Noised.Core.Media.NoisedMetaFile;
using Noised.Core.Plugins;
using NUnit.Framework;

namespace NoisedTests.Media
{
    [TestFixture]
    public class MetaFileAccumulatorTests
    {
        [Test]
        public void MetaFileAccumulator_ConstructorWithoutPluginLoader_ShouldThrowException()
        {
            try
            {
                var dbFactoryMock = new Mock<IDbFactory>();
                var metaFileWriterMock = new Mock<IMetaFileWriter>();
                var mediaSourceAccumulator = new Mock<IMediaSourceAccumulator>();

                new MetaFileAccumulator(null, dbFactoryMock.Object, metaFileWriterMock.Object, mediaSourceAccumulator.Object);
                Assert.Fail("Expected ArgumentNullException.");
            }
            catch(ArgumentNullException e)
            {
                Assert.AreSame("pluginLoader", e.ParamName);
            }
        }

        [Test]
        public void MetaFileAccumulator_ConstructorWithoutDbFactory_ShouldThrowException()
        {
            try
            {
                var pluginLoaderMock = new Mock<IPluginLoader>();
                var metaFileWriterMock = new Mock<IMetaFileWriter>();
                var mediaSourceAccumulator = new Mock<IMediaSourceAccumulator>();

                new MetaFileAccumulator(pluginLoaderMock.Object, null, metaFileWriterMock.Object, mediaSourceAccumulator.Object);
                Assert.Fail("Expected ArgumentNullException.");
            }
            catch(ArgumentNullException e)
            {
                Assert.AreSame("dbFactory", e.ParamName);
            }
        }

        [Test]
        public void MetaFileAccumulator_ConstructorWithoutMetaFileWriter_ShouldThrowException()
        {
            try
            {
                var pluginLoaderMock = new Mock<IPluginLoader>();
                var dbFactoryMock = new Mock<IDbFactory>();
                var mediaSourceAccumulator = new Mock<IMediaSourceAccumulator>();

                new MetaFileAccumulator(pluginLoaderMock.Object, dbFactoryMock.Object, null, mediaSourceAccumulator.Object);
                Assert.Fail("Expected ArgumentNullException.");
            }
            catch(ArgumentNullException e)
            {
                Assert.AreSame("metaFileWriter", e.ParamName);
            }
        }

        [Test]
        public void MetaFileAccumulator_ConstructorWithoutMediaSourceAccumulator_ShouldThrowException()
        {
            try
            {
                var pluginLoaderMock = new Mock<IPluginLoader>();
                var dbFactoryMock = new Mock<IDbFactory>();
                var metaFileWriterMock = new Mock<IMetaFileWriter>();

                new MetaFileAccumulator(pluginLoaderMock.Object, dbFactoryMock.Object, metaFileWriterMock.Object, null);
                Assert.Fail("Expected ArgumentNullException.");
            }
            catch(ArgumentNullException e)
            {
                Assert.AreSame("mediaSourceAccumulator", e.ParamName);
            }
        }

        [Test]
        public void MetaFileAccumulator_Constructor_CanCreateInstance()
        { }
    }
}
