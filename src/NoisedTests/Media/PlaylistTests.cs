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
        }

        [Test]
        public void CreatePlaylistTest()
        {
            Playlist p = pman.CreatePlaylist("playlistName");
            p.Name.ShouldEqual("playlistName");
        }
    }
}
