using NUnit.Framework;
using Should;

namespace NoisedTests
{
	[TestFixture]
	public class Test
	{
		[Test]
		public void ShouldNotFail()
		{
			true.ShouldBeTrue();	
		}
	}
}
