using System;
using Moq;
using NUnit.Framework;
using Noised.Core.Commands;
using Noised.Core.Media;
using Noised.Plugins.Commands.CoreCommands;

namespace NoisedTests.Commands
{
    [TestFixture]
    public class RepeatModeTest : AbstractCommandTest
    {

        [Test]
        public void GetRepeatModeCommandShouldReturnRepeatMode()
        {
            var mediaManager = new Mock<IMediaManager>();
            RegisterToDIMock<IMediaManager>(mediaManager.Object);

            new GetRepeatMode(ContextMock.Object).ExecuteCommand();

            mediaManager.VerifyGet(mm => mm.Repeat);
            string expectedResponseName = "Noised.Plugins.Commands.CoreCommands.GetRepeatMode";
            ContextMock.Verify(ctx => ctx.SendResponse(
                        It.Is<ResponseMetaData>(r => r.Name == expectedResponseName)));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException), UserMessage = "GetRepeatMode command should throw an ArgumentNullException when invoked without IServiceConnectionContext.")]
        public void GetRepeatModeArgumentNullException()
        {
            new GetRepeatMode(null);
        }

        [Test]
        public void SetRepeatSongModeShouldSetRepeatMode()
        {

            var mediaManager = new Mock<IMediaManager>();
            RegisterToDIMock<IMediaManager>(mediaManager.Object);

            new SetRepeatMode(ContextMock.Object, "RepeatSong").ExecuteCommand();
            mediaManager.VerifySet(mm => mm.Repeat = RepeatMode.RepeatSong); 
        }

        [Test]
        public void SetRepeatPlaylistModeShouldSetRepeatMode()
        {
            var mediaManager = new Mock<IMediaManager>();
            RegisterToDIMock<IMediaManager>(mediaManager.Object);

            new SetRepeatMode(ContextMock.Object, "RepeatPlaylist").ExecuteCommand();
            mediaManager.VerifySet(mm => mm.Repeat = RepeatMode.RepeatPlaylist); 
        }

        [Test]
        [ExpectedException(typeof (ArgumentException), UserMessage = "Trying to set an invalid RepeatMode should result in an ArgumentException.")]
        public void SetRepeatModeArgumentException()
        {
            new SetRepeatMode(ContextMock.Object, "InvalidRepeatMode");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException),
            UserMessage =
                "SetRepeatMode command should throw an ArgumentNullException when invoked without IServiceConnectionContext."
            )]
        public void SetRepeatModeArgumentNullException()
        {
            new SetRepeatMode(null, "None");
        }
    }
}
