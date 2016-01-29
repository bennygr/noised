using Noised.Core.Media;

namespace Noised.Core.DB
{
    public interface IPlaylistRepository
    {
        void CreatePlaylist(Playlist playlist);

        Playlist ReadPlaylist(string name);

        void UpdatePlaylist(Playlist playlist);

        void DeletePlaylist(Playlist playlist);
    }
}
