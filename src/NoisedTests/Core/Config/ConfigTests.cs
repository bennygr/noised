using System;
using Moq;
using NUnit.Framework;
using Noised.Core.Config;
using Noised.Logging;
using Should;

namespace NoisedTests.Core.Config
{
    /// <summary>
    ///     Test fixtures for testing configuration behaviour
    /// </summary>
    [TestFixture]
    public class ConfigTests
    {

        /// <summary>
        ///     Noised should be able to load an empty configuration without errors
        /// </summary>
        [Test]
        public void ShouldLoadEmptyConfiguration()
        {
            //Setting up an config loader which returns am empty configruation
            var configLoader = new Mock<IConfigurationLoader>();
            configLoader.Setup(x => x.LoadData()).Returns(new System.Collections.Generic.List<ConfigurationData>());

            //Setting up the config which we want to test
            var logging = new Mock<ILogging>();
            IConfig config = new Noised.Core.Config.Config(logging.Object);

            //Test to load an emtpy configuration
            config.Load(configLoader.Object);
            config.Count.ShouldEqual(0);
        }

        /// <summary>
        ///	Noised should be able to load multiple configuration properties correctly
        /// </summary>
        [Test]
        public void ShouldLoadMultipleConfigurationProperties()
        {
            //Setting up a simple property source
            var simpleMockLoader = new Mock<IConfigurationLoader>();
            simpleMockLoader.Setup(x => x.LoadData()).Returns(
                new System.Collections.Generic.List<ConfigurationData>
                {
                    new ConfigurationData
                    {
                        Name = "Test.nconfig",
                        Data = "#This comment should not be parsed " + Environment.NewLine +
                        "Noised.Test.SimpleConfiguration.Value1=1" + Environment.NewLine +
                        "Noised.Test.SimpleConfiguration.Value2=2"
                    },
                    new ConfigurationData
                    {
                        Name = "Test2.nconfig",
                        Data = "#This comment should not be parsed " + Environment.NewLine +
                        "Noised.Test.SimpleConfiguration.Value3=3" + Environment.NewLine +
                        "Noised.Test.SimpleConfiguration.Value4=4"
                    }
                });

            //Setting up the config which we want to test
            var logging = new Mock<ILogging>();
            IConfig config = new Noised.Core.Config.Config(logging.Object);

            config.Load(simpleMockLoader.Object);
            config.Count.ShouldEqual(4);

            //Checking values
            var value1 = config.GetProperty("Noised.Test.SimpleConfiguration.Value1");
            value1.ShouldEqual("1");
            var value2 = config.GetProperty("Noised.Test.SimpleConfiguration.Value2");
            value2.ShouldEqual("2");
            var value3 = config.GetProperty("Noised.Test.SimpleConfiguration.Value3");
            value3.ShouldEqual("3");
            var value4 = config.GetProperty("Noised.Test.SimpleConfiguration.Value4");
            value4.ShouldEqual("4");

            //value should not exists
            var value5 = config.GetProperty("Property.Which.Does.Not.Exist");
            value5.ShouldBeNull();

            //value should also not exists
            var value6 = config.GetProperty("Property.Which.Does.Not.Exist", "DefaultValue");
            value6.ShouldEqual("DefaultValue");
        }

        /// <summary>
        ///     Noised should allow property overwriting
        /// </summary>
        [Test]
        public void ShouldOverwriteIfPropertyIsSetMoreThanOnce()
        {
            //Setting up a config which overwrites an already defined value existing
            var simpleMockLoader = new Mock<IConfigurationLoader>();
            simpleMockLoader.Setup(x => x.LoadData()).Returns(
                new System.Collections.Generic.List<ConfigurationData>
                {
                    new ConfigurationData
                    {
                        Name = "Test.nconfig",
                        Data = "#This comment should not be parsed " + Environment.NewLine +
                        "Noised.Test.SimpleConfiguration.Value1=old" + Environment.NewLine +
                        "Noised.Test.SimpleConfiguration.Value1=new"
                    },
                    new ConfigurationData
                    {
                        Name = "Test.nconfig",
                        Data = "#This comment should not be parsed " + Environment.NewLine +
                        "Noised.Test.SimpleConfiguration.Value1=newer" + Environment.NewLine +
                        "Noised.Test.SimpleConfiguration.Value1=latest"
                    },
                });

            //Setting up the config which we want to test
            var logging = new Mock<ILogging>();
            IConfig config = new Noised.Core.Config.Config(logging.Object);

            config.Load(simpleMockLoader.Object);
            config.Count.ShouldEqual(1);

            var value1 = config.GetProperty("Noised.Test.SimpleConfiguration.Value1");
            value1.ShouldEqual("latest");
        }
    };
}
