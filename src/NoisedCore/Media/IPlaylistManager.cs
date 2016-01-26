using System.Collections.ObjectModel;

namespace Noised.Core.Media
{
    public interface IPlaylistManager
    {
        ReadOnlyCollection<Playlist> Playlists { get; }
        Playlist LoadedPlaylist { get; }
        Playlist CreatePlaylist(string name);
        void AddPlaylist(Playlist playlist);
        Playlist FindPlaylists(string name);
        void LoadPlaylist(Playlist playlist);
    }
}