using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Noised.Core.DB;
using Noised.Core.IOC;

namespace Noised.Core.Media
{
    public class PlaylistManager : IPlaylistManager
    {
        private readonly List<Playlist> playlists;
        private readonly IPlaylistRepository playlistRepository;

        public ReadOnlyCollection<Playlist> Playlists
        {
            get
            {
                return new ReadOnlyCollection<Playlist>(playlists);
            }
        }

        public Playlist LoadedPlaylist { get; private set; }

        public PlaylistManager()
        {
            playlistRepository = IocContainer.Get<IUnitOfWork>().PlaylistRepository;
            playlists = playlistRepository.GetAllPlaylists();
        }

        public Playlist CreatePlaylist(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException(strings.NoValidPlaylistName, "name");

            return new Playlist(name);
        }

        public void AddPlaylist(Playlist playlist)
        {
            if (playlist == null)
                throw new ArgumentNullException("playlist");
            if (playlists.Any(x => x.Name == playlist.Name))
                throw new PlaylistAlreadyExistsException("A Playlist with this name already exists.");

            playlistRepository.CreatePlaylist(playlist);
            playlists.Add(playlist);
        }

        public Playlist FindPlaylist(string name)
        {
            return playlists.Find(x => x.Name == name);
        }

        public void LoadPlaylist(Playlist playlist)
        {
            LoadedPlaylist = playlist;
        }

        public void DeletePlaylist(Playlist playlist)
        {
            playlists.Remove(playlist);
            playlistRepository.DeletePlaylist(playlist);
        }

        /// <summary>
        /// Saves a Playlist to the configured medium
        /// </summary>
        /// <param name="playlist"></param>
        public void SavePlaylist(Playlist playlist)
        {
            playlistRepository.UpdatePlaylist(playlist);
        }
    }
}
