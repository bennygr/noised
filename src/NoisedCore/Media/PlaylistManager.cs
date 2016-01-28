using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Noised.Core.Media
{
    public class PlaylistManager : IPlaylistManager
    {
        private readonly List<Playlist> playlists;

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
            playlists = new List<Playlist>();
        }

        public Playlist CreatePlaylist(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Please provide a valid (non empty) name for the playlist.", "name");

            return new Playlist(name);
        }

        public void AddPlaylist(Playlist playlist)
        {
            if (playlist == null)
                throw new ArgumentNullException("playlist");
            if (playlists.Any(x => x.Name == playlist.Name))
                throw new PlaylistAlreadyExistsException("A Playlist with this name already exists.");

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
    }
}
