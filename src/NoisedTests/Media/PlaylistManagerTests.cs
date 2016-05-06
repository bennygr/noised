using NUnit.Framework;
using Noised.Core.Media;
using Should;

namespace NoisedTests.Media
{
    [TestFixture]
    public class PlaylistManagerTests
    {
        [Test]
        public void LoadingAPlaylistShouldSetTheLoadedPlaylist()
        {
            var playlistManager = new PlaylistManager();
            var playlist1 = new Playlist("testlist");
            var playlist2 = new Playlist("testlist2");

            playlistManager.LoadPlaylist(playlist1);
            playlistManager.LoadedPlaylist.ShouldEqual(playlist1, "Loaded Playlist does not equal the Playlists that was loaded.");

            playlistManager.LoadPlaylist(playlist2);
            playlistManager.LoadedPlaylist.ShouldEqual(playlist2, "Loaded Playlist does not equal the Playlists that was loaded.");
        }
    };
}
