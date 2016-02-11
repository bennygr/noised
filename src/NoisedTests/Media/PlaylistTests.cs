using System;
using System.Collections.Generic;
using Moq;
using Noised.Core.DB;
using Noised.Core.Media;
using NUnit.Framework;
using Should;

namespace NoisedTests.Media
{
    [TestFixture]
    public class PlaylistTests
    {
        private IPlaylistManager pman;

        [SetUp]
        public void PlaylistTestsSetup()
        {            
            Mock<IPlaylistRepository> mockPlaylistRepository = new Mock<IPlaylistRepository>();
            mockPlaylistRepository.Setup(x => x.GetAllPlaylists()).Returns(new List<Playlist>());
            
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.PlaylistRepository).Returns(mockPlaylistRepository.Object);

            Mock<IDbFactory> mockFactory = new Mock<IDbFactory>();
            mockFactory.Setup(x => x.GetUnitOfWork()).Returns(mockUnitOfWork.Object);

            pman = new PlaylistManager(mockFactory.Object);
            pman.RefreshPlaylists();
        }

        [Test]
        public void CreatePlaylistTest()
        {
            pman.CreatePlaylist("playlistName").Name.ShouldEqual("playlistName");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CreatePlaylistArgumentExceptionTest()
        {
            pman.CreatePlaylist(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddPlaylistArgumentExceptionTest()
        {
            pman.AddPlaylist(null);
        }

        [Test]
        [ExpectedException(typeof(PlaylistAlreadyExistsException))]
        public void AddPlaylistPlaylistAlreadyExistsExceptionTest()
        {
            pman.AddPlaylist(pman.CreatePlaylist("testlist"));
            pman.AddPlaylist(pman.CreatePlaylist("testlist"));
        }

        [Test]
        public void AddPlaylistTest()
        {
            pman.Playlists.Count.ShouldEqual(0);

            pman.AddPlaylist(pman.CreatePlaylist("testlist"));

            pman.Playlists.Count.ShouldEqual(1);

            pman.Playlists[0].Name.ShouldEqual("testlist");
        }

        [Test]
        public void FindPlaylistTest()
        {
            pman.FindPlaylist("testlist").ShouldBeNull();

            Playlist plist = pman.CreatePlaylist("testlist");
            pman.AddPlaylist(plist);

            pman.FindPlaylist("testlist").ShouldNotBeNull();
            pman.FindPlaylist("testlist").Name.ShouldEqual("testlist");
            pman.FindPlaylist("testlist").ShouldEqual(plist);
        }

        [Test]
        public void LoadPlaylistTest()
        {
            Playlist plist = pman.CreatePlaylist("testlist");
            Playlist plist2 = pman.CreatePlaylist("testlist2");

            pman.LoadPlaylist(plist);
            pman.LoadedPlaylist.ShouldEqual(plist);

            pman.LoadPlaylist(plist2);
            pman.LoadedPlaylist.ShouldEqual(plist2);
        }

        [Test]
        public void DeletePlaylistTest()
        {
            Playlist plist = pman.CreatePlaylist("testlist");

            pman.AddPlaylist(plist);
            pman.Playlists.Count.ShouldEqual(1);

            pman.DeletePlaylist(plist);
            pman.Playlists.Count.ShouldEqual(0);
        }

        [Test]
        public void SavePlaylistTest()
        {
            pman.SavePlaylist(pman.CreatePlaylist("testlist"));
        }
    }
}
