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

		[Test]
		public void TestLoadEmptyConfiguration()
		{
			//Testing no configuration
			var emptySourceMock = new Mock<IConfigSource>();
			emptySourceMock.Setup(x => x.GetSourceData()).Returns("");
			config.Load(emptySourceMock.Object);
			config.Count().ShouldEqual(0);
		}

		[Test]
		public void TestLoadSimpleConfiguration()
		{
			//Setting up a simple property source
			var simpleSourceMock = new Mock<IConfigSource>();
			simpleSourceMock.Setup(x => x.GetSourceData()).Returns(
					"#This comment should not be parsed " + Environment.NewLine + 
					"Noised.Test.SimpleConfiguration.Value1=1" + Environment.NewLine + 
					"Noised.Test.SimpleConfiguration.Value2=2"
					);
			config.Load(simpleSourceMock.Object);
			config.Count().ShouldEqual(2);

			//Checking value 1
			var value1 = config.GetProperty("Noised.Test.SimpleConfiguration.Value1");
			value1.ShouldEqual("1");

			//Checking value 2
			var value2 = config.GetProperty("Noised.Test.SimpleConfiguration.Value2");
			value2.ShouldEqual("2");

			//value 3 should not exists
			var value3 = config.GetProperty("Property.Which.Does.Not.Exist");
			value3.ShouldBeNull();
			
			//value 4 should also not exists
			var value4 = config.GetProperty("Property.Which.Does.Not.Exist","DefaultValue");
			value4.ShouldEqual("DefaultValue");
		}
	};
}
