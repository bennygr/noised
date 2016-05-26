using System;
using System.Collections.Generic;
using Moq;
using Noised.Core.Commands;
using Noised.Core.DB;
using Noised.Core.Media;
using Noised.Plugins.Commands.CoreCommands;
using NUnit.Framework;
using Should;

namespace NoisedTests.Commands
{
    [TestFixture]
    public class PlaylistCommandsTests : AbstractCommandTest
    {
        [Test]
        public void CreatePlaylistCommandShouldPutPlaylistInRepository()
        {
            var playlistRepository = new Mock<IPlaylistRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(m => m.PlaylistRepository).Returns(playlistRepository.Object);
            RegisterToDIMock<IUnitOfWork>(unitOfWork.Object);

            new CreatePlaylist(ContextMock.Object, "testList").ExecuteCommand();

            playlistRepository.Verify(m => m.Create(It.Is<Playlist>(p => p.Name == "testList")));
            unitOfWork.Verify(u => u.SaveChanges());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreatePlaylistCommandArgumentNullExceptionTest()
        {
            new CreatePlaylist(null, "CreatePlaylistCommandTest").ExecuteCommand();
        }

        [Test]
        public void CreatePlaylistWithInvalidNameShouldSendErrorToClient()
        {
            try
            {
                new CreatePlaylist(ContextMock.Object, String.Empty).ExecuteCommand();
            }
            catch (Exception e)
            {
                e.ShouldBeType<ArgumentException>();
            }

            var expectedResponseCommand = "Noised.Commands.Core.CreatePlaylist";
            ContextMock.Verify(m => m.SendResponse(It.Is<ResponseMetaData>(
                        r => r.Name == expectedResponseCommand &&
                        r.Parameters.Count == 1)));
        }

        [Test]
        public void DeletePlaylistCommandTest()
        {
            var playlistRepository = new Mock<IPlaylistRepository>();
            playlistRepository.Setup(r => r.GetById(117)).Returns(new Playlist("testList") { Id = 117 });
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(m => m.PlaylistRepository).Returns(playlistRepository.Object);
            RegisterToDIMock(unitOfWork.Object);

            new DeletePlaylist(ContextMock.Object, 117).ExecuteCommand();

            playlistRepository.Verify(r => r.Delete(It.Is<Playlist>(p => p.Id == 117)));
            unitOfWork.Verify(u => u.SaveChanges());
        }

        [Test]
        public void DeleteUnknownPlaylistShouldSendErrorToClient()
        {
            var playlistRepository = new Mock<IPlaylistRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(m => m.PlaylistRepository).Returns(playlistRepository.Object);
            RegisterToDIMock<IUnitOfWork>(unitOfWork.Object);

            new DeletePlaylist(ContextMock.Object, 117).ExecuteCommand();

            String expectedErrorResponse = "Noised.Plugins.Commands.CoreCommands.DeletePlaylist";
            ContextMock.Verify(c => c.SendResponse(It.Is<ErrorResponse>(e => e.Name == expectedErrorResponse)));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeletePlaylistCommandArgumentNullExceptionTest()
        {
            new DeletePlaylist(null, 0).ExecuteCommand();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveFromPlaylistArgumentNullExceptionTest()
        {
            new RemoveFromPlaylist(null, 0, new List<string>()).ExecuteCommand();
        }

        [Test]
        public void RemoveEmptyItemsFromPlaylistShouldSendErrorToClient()
        {
            try
            {
                new RemoveFromPlaylist(ContextMock.Object, 0, new List<string>());
            }
            catch (Exception e)
            {
                e.ShouldBeType<ArgumentNullException>();
            }

            String expectedErrorResponse = "Noised.Plugins.Commands.CoreCommands.RemoveFromPlaylist";
            ContextMock.Verify(c => c.SendResponse(It.Is<ErrorResponse>(e => e.Name == expectedErrorResponse)));
        }

        [Test]
        public void RemoveNullItemsFromPlaylistShouldSendErrorToClient()
        {
            try
            {
                new RemoveFromPlaylist(ContextMock.Object, 0, null);
            }
            catch (Exception e)
            {
                e.ShouldBeType<ArgumentNullException>();
            }

            String expectedErrorResponse = "Noised.Plugins.Commands.CoreCommands.RemoveFromPlaylist";
            ContextMock.Verify(c => c.SendResponse(It.Is<ErrorResponse>(e => e.Name == expectedErrorResponse)));
        }

        [Test]
        public void AddToPlaylistShouldAddAnItemToAPlaylist()
        {
            var playlistRepository = new Mock<IPlaylistRepository>();
            Playlist playlist = new Playlist("testList") { Id = 1337 };
            playlistRepository.Setup(r => r.GetById(1337)).Returns(playlist);
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(m => m.PlaylistRepository).Returns(playlistRepository.Object);
            RegisterToDIMock<IUnitOfWork>(unitOfWork.Object);
            var msa = new Mock<IMediaSourceAccumulator>();
            string path = @"C:\test\test.mp3";
            msa.Setup(x => x.Get(new Uri(path))).Returns(new MediaItem(new Uri(path), String.Empty));
            RegisterToDIMock(msa.Object);


            new AddToPlaylist(ContextMock.Object, 1337, new List<object> { path }).ExecuteCommand();


        }
    }
}
