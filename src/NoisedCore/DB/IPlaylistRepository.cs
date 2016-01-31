using System.Collections.Generic;
using Noised.Core.Media;

namespace Noised.Core.DB
{
    public interface IPlaylistRepository
    {
        void CreatePlaylist(Playlist playlist);

        void UpdatePlaylist(Playlist playlist);

        void DeletePlaylist(Playlist playlist);

        List<Playlist> GetAllPlaylists();
    }
}
