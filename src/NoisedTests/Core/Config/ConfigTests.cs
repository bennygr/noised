using NUnit.Framework;
using Noised.Core.Config;
using Noised.Core.IOC;
using Should;

namespace Noised.Tests.Core.Config
{
	[TestFixture]
	public class ConfigTests
	{
		[Test]
		public void Test()
		{
			IocContainer.Build();
			var config = IocContainer.Get<IConfig>();

			config.GetProperty("Unknown.Property").ShouldBeNull();
			config.GetProperty("Unknown.Property","default").ShouldEqual("default");
		}
	};
}
