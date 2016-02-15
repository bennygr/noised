using Moq;
using Noised.Core;
using Noised.Core.Commands;
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
        [TestFixtureSetUp]
        public void ShuffleStatusTestSetUp()
        {
            IocContainer.Build();
        }

        [Test]
        public void SetShuffleStatusTest()
        {
            Mock<IServiceConnectionContext> serviceConnectionMock = new Mock<IServiceConnectionContext>();

            IocContainer.Get<ICore>().ExecuteCommand(new SetShuffleStatus(serviceConnectionMock.Object, false));

            IocContainer.Get<IMediaManager>().Shuffle.ShouldBeFalse();

            IocContainer.Get<ICore>().ExecuteCommand(new SetShuffleStatus(serviceConnectionMock.Object, true));

            IocContainer.Get<IMediaManager>().Shuffle.ShouldBeTrue();
        }

        [Test]
        public void GetShuffleStatusTest()
        {
            ResponseMetaData responseMetaData = new ResponseMetaData();
            Mock<IServiceConnectionContext> serviceConnectionMock = new Mock<IServiceConnectionContext>();
            serviceConnectionMock.Setup(x => x.SendResponse(It.IsAny<ResponseMetaData>())).Callback((ResponseMetaData r) => responseMetaData = r);

            new GetShuffleStatus(serviceConnectionMock.Object).ExecuteCommand();

            responseMetaData.Parameters[0].ShouldEqual(false);

            IocContainer.Get<IMediaManager>().Shuffle = true;

            new GetShuffleStatus(serviceConnectionMock.Object).ExecuteCommand();

            responseMetaData.Parameters[0].ShouldEqual(true);
        }
    }
}
