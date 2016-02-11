using System.Collections.Generic;
using Moq;
using Noised.Core.DB;
using Noised.Core.IOC;
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
            IocContainer.Build();
            pman = IocContainer.Get<IPlaylistManager>();

            Mock<IDbFactory> mockFactory = new Mock<IDbFactory>();
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            Mock<IPlaylistRepository> mockPlaylistRepository = new Mock<IPlaylistRepository>();

            mockPlaylistRepository.Setup(x => x.GetAllPlaylists()).Returns(new List<Playlist>());

            mockUnitOfWork.Setup(x => x.PlaylistRepository).Returns(mockPlaylistRepository.Object);

            mockFactory.Setup(x => x.GetUnitOfWork()).Returns(mockUnitOfWork.Object);

            pman.DbFactory = mockFactory.Object;
            pman.RefreshPlaylists();
        }

        [Test]
        public void CreatePlaylistTest()
        {
            Playlist p = pman.CreatePlaylist("playlistName");
            p.Name.ShouldEqual("playlistName");
        }
    }
}
