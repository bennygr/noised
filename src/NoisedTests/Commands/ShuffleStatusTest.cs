using Moq;
using Noised.Core;
using Noised.Core.IOC;
using Noised.Core.Media;
using Noised.Core.Service;
using Noised.Plugins.Commands.CoreCommands;
using NUnit.Framework;
using Should;

namespace NoisedTests.Commands
{
    [TestFixture]
    public class ShuffleStatusTest
    {
        [SetUp]
        public void ShuffleStatusTestSetUp()
        {
            IocContainer.Build();
        }

        [Test]
        public void SetShuffleStatusTest()
        {
            IocContainer.Get<IMediaManager>().Shuffle.ShouldBeFalse();
            
            IocContainer.Get<ICore>().ExecuteCommand(new SetShuffleStatus(new Mock<IServiceConnectionContext>().Object, true));

            IocContainer.Get<IMediaManager>().Shuffle.ShouldBeTrue();
        }

        [Test]
        public void GetShuffleStatusTest()
        {
            
        }
    }
}
