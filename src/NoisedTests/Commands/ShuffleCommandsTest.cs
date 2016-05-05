using System;
using Moq;
using NUnit.Framework;
using Noised.Core.Commands;
using Noised.Core.Media;
using Noised.Plugins.Commands.CoreCommands;
using Should;

namespace NoisedTests.Commands
{
    [TestFixture]
    public class ShuffleCommandsTest : AbstractCommandTest
    {
        [Test]
        public void SetShuffleStatusCommandShouldSetShuffleMode()
        {
            var mediaManager = new Mock<IMediaManager>();
            RegisterToDIMock<IMediaManager>(mediaManager.Object);

            new SetShuffleStatus(ContextMock.Object, false).ExecuteCommand();
            mediaManager.VerifySet(mm => mm.Shuffle = false, "Shufle status should have been set to false");
            new SetShuffleStatus(ContextMock.Object, true).ExecuteCommand();
            mediaManager.VerifySet(mm => mm.Shuffle = true, "Shufle status should have been set to false");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException), UserMessage = "SetShuffleStatus command should throw an ArgumentNullException when invoked without IServiceConnectionContext.")]
        public void SetShuffleStatusCommandArgumentNullException()
        {
            new SetShuffleStatus(null, false);
        }

        [Test]
        public void GetShuffleStatusCommandShouldGetShuffleMode()
        {
            var mediaManager = new Mock<IMediaManager>();
            RegisterToDIMock<IMediaManager>(mediaManager.Object);

            new GetShuffleStatus(ContextMock.Object).ExecuteCommand();

            mediaManager.VerifyGet(mm => mm.Shuffle);
            string expectedResponseName = "Noised.Plugins.Commands.CoreCommands.GetShuffleStatus";
            ContextMock.Verify(ctx => ctx.SendResponse(
                        It.Is<ResponseMetaData>(r => r.Name == expectedResponseName)));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException), UserMessage = "GetShuffleStatus command should throw an ArgumentNullException when invoked without IServiceConnectionContext.")]
        public void GetShuffleStatusArgumentNullException()
        {
            new GetShuffleStatus(null);
        }
    }
}
