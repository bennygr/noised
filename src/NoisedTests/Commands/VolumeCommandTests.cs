using Moq;
using NUnit.Framework;
using Noised.Core.Commands;
using Noised.Core.Media;
using Noised.Plugins.Commands.CoreCommands;

namespace NoisedTests.Commands
{
    [TestFixture]
    public class VolumeCommandTests : AbstractCommandTest
    {
        [Test]
        public void SetVolumeTest()
        {
            var mediaManager = new Mock<IMediaManager>();
            RegisterToDIMock<IMediaManager>(mediaManager.Object);
            new SetVolume(ContextMock.Object, 50).ExecuteCommand();
            mediaManager.VerifySet(mm => mm.Volume = 50);
        }

        [Test]
        public void GetVolumeTest()
        {
            var mediaManager = new Mock<IMediaManager>();
            RegisterToDIMock<IMediaManager>(mediaManager.Object);

            new GetVolume(ContextMock.Object).ExecuteCommand();

            mediaManager.VerifyGet(mm => mm.Volume);
            string expectedResponseName = "Noised.Plugins.Commands.CoreCommands.GetVolume";
            ContextMock.Verify(ctx => ctx.SendResponse(
                        It.Is<ResponseMetaData>(r => r.Name == expectedResponseName)));
        }
    }
}
