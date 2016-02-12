using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Noised.Core.DB;

namespace Noised.Core.Media
{
    /// <summary>
    /// Manages all the Playlists
    /// </summary>
    public class PlaylistManager : IPlaylistManager
    {
        private List<Playlist> playlists;
        private IDbFactory dbFactory;

        /// <summary>
        /// Gets all the Playlists
        /// </summary>
        public ReadOnlyCollection<Playlist> Playlists
        {
            get
            {
                return new ReadOnlyCollection<Playlist>(playlists);
            }
        }

        /// <summary>
        /// Gets the currently loaded Playlist
        /// </summary>
        public Playlist LoadedPlaylist { get; private set; }

        /// <summary>
        /// Manages all the Playlists
        /// </summary>
        /// <param name="dbFactory">A Factory for creating instances of IUnitOfWork to access the Database</param>
        public PlaylistManager(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
            playlists = new List<Playlist>();
        }

        /// <summary>
        /// Creates a Playlist
        /// </summary>
        /// <param name="name">Name of the Playlist to create</param>
        /// <returns>The created Playlist</returns>
        public Playlist CreatePlaylist(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException(strings.NoValidPlaylistName, "name");

            return new Playlist(name);
        }

        /// <summary>
        /// Adds a Playlist to the Playlistmanager
        /// </summary>
        /// <param name="playlist"></param>
        public void AddPlaylist(Playlist playlist)
        {
            if (playlist == null)
                throw new ArgumentNullException("playlist");

            lock (playlists)
            {
                if (playlists.Any(x => x.Name == playlist.Name))
                    throw new PlaylistAlreadyExistsException("A Playlist with this name already exists.");

                using (IUnitOfWork uow = DbFactory.GetUnitOfWork())
                {
                    uow.PlaylistRepository.CreatePlaylist(playlist);
                    uow.SaveChanges();
                }

                playlists.Add(playlist);
            }
        }

        /// <summary>
        /// Searches and return a Playlist by name
        /// </summary>
        /// <param name="name">The name of the playlist</param>
        /// <returns>The Playlist with the given name or null</returns>
        public Playlist FindPlaylist(string name)
        {
            lock (playlists)
                return playlists.Find(x => x.Name == name);
        }

        /// <summary>
        /// Loads a Playlist
        /// </summary>
        /// <param name="playlist">The Playlist to load</param>
        public void LoadPlaylist(Playlist playlist)
        {
            lock (playlists)
            {
                playlist.ResetAlreadyPlayedItems();
                LoadedPlaylist = playlist;
            }
        }

        /// <summary>
        /// Deletes a Playlist
        /// </summary>
        /// <param name="playlist">Playlist to delete</param>
        public void DeletePlaylist(Playlist playlist)
        {
            lock (playlists)
            {
                playlists.Remove(playlist);
                using (IUnitOfWork uow = DbFactory.GetUnitOfWork())
                {
                    uow.PlaylistRepository.DeletePlaylist(playlist);
                    uow.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Saves a Playlist to the configured medium
        /// </summary>
        /// <param name="playlist"></param>
        public void SavePlaylist(Playlist playlist)
        {
            lock (playlists)
            {
                using (IUnitOfWork uow = DbFactory.GetUnitOfWork())
                {
                    uow.PlaylistRepository.UpdatePlaylist(playlist);
                    uow.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Sets a Factory for creating instances of IUnitOfWork to access the Database
        /// </summary>
        public IDbFactory DbFactory
        {
            private get
            {
                if (dbFactory == null)
                    throw new PlaylistManagerException("You need to set the DbFactory Property first!");

                return dbFactory;
            }
            set
            {
                dbFactory = value;
            }
        }

        /// <summary>
        /// Loads all Playlists
        /// </summary>
        public void RefreshPlaylists()
        {
            lock (playlists)
            {
                using (IUnitOfWork uow = DbFactory.GetUnitOfWork())
                    playlists = uow.PlaylistRepository.AllPlaylists.ToList();
            }
        }
    }
}
