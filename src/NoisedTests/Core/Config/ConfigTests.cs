using System;
using Moq;
using NUnit.Framework;
using Noised.Core.Config;
using Noised.Core.IOC;
using Should;

namespace Noised.Tests.Core.Config
{
	/// <summary>
	///		Test fixtures for testing configuration behaviour
	/// </summary>
	[TestFixture]
	public class ConfigTests
	{
		private IConfig config;

		[SetUp]
		public void RunBeforeAnyTests()
		{
			//building configuration object on
			IocContainer.Build();
			config = IocContainer.Get<IConfig>();
		}

		/// <summary>
		///		Noised should be able to load an empty configuration without errors
		/// </summary>
		[Test]
		public void ShouldLoadEmptyConfiguration()
		{
			//Testing an empty configuration
			var mockLoader = new Mock<IConfigurationLoader>();
			mockLoader.Setup(x => x.LoadData()).Returns(new System.Collections.Generic.List<ConfigurationData>());
			config.Load(mockLoader.Object);
			config.Count.ShouldEqual(0);
		}

		/// <summary>
		///		Noised should be able to load multiple configuration properties correctly
		/// </summary>
		[Test]
		public void ShouldLoadMultipleConfigurationProperties()
		{
			//Setting up a simple property source
			var simpleMockLoader = new Mock<IConfigurationLoader>();
			simpleMockLoader.Setup(x => x.LoadData()).Returns(
					new System.Collections.Generic.List<ConfigurationData> 
					{
						new ConfigurationData{
							Name = "Test.nconfig",
							Data = "#This comment should not be parsed " + Environment.NewLine + 
								   "Noised.Test.SimpleConfiguration.Value1=1" + Environment.NewLine + 
								   "Noised.Test.SimpleConfiguration.Value2=2"
						},
						new ConfigurationData{
							Name = "Test2.nconfig",
							Data = "#This comment should not be parsed " + Environment.NewLine + 
								   "Noised.Test.SimpleConfiguration.Value3=3" + Environment.NewLine + 
								   "Noised.Test.SimpleConfiguration.Value4=4"
						}
					});
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
			var value6 = config.GetProperty("Property.Which.Does.Not.Exist","DefaultValue");
			value6.ShouldEqual("DefaultValue");
		}

		/// <summary>
		///		Noised should allow property overwriting
		/// </summary>
		[Test]
		public void ShouldOverwriteIfPropertyIsSetMoreThanOnce()
		{
			//Setting up a config which overwrites an already defined value existing
			var simpleMockLoader = new Mock<IConfigurationLoader>();
			simpleMockLoader.Setup(x => x.LoadData()).Returns(
					new System.Collections.Generic.List<ConfigurationData> 
					{
						new ConfigurationData{
							Name = "Test.nconfig",
							Data = "#This comment should not be parsed " + Environment.NewLine + 
								   "Noised.Test.SimpleConfiguration.Value1=old" + Environment.NewLine + 
								   "Noised.Test.SimpleConfiguration.Value1=new"
						},
						new ConfigurationData{
							Name = "Test.nconfig",
							Data = "#This comment should not be parsed " + Environment.NewLine + 
								   "Noised.Test.SimpleConfiguration.Value1=newer" + Environment.NewLine + 
								   "Noised.Test.SimpleConfiguration.Value1=latest"
						},
					});

			config.Load(simpleMockLoader.Object);
			config.Count.ShouldEqual(1);

			var value1 = config.GetProperty("Noised.Test.SimpleConfiguration.Value1");
			value1.ShouldEqual("latest");

			throw new Exception("Test-Exception for testing CI notificaions");
		}
	};
}
