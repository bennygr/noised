using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Noised.Core.DB;

namespace Noised.Core.Media
{
    public class PlaylistManager : IPlaylistManager
    {
        private List<Playlist> playlists;
        private IDbFactory dbFactory;

        public ReadOnlyCollection<Playlist> Playlists
        {
            get
            {
                return new ReadOnlyCollection<Playlist>(playlists);
            }
        }

        public Playlist LoadedPlaylist { get; private set; }

        public PlaylistManager(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
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

        public Playlist FindPlaylist(string name)
        {
            lock (playlists)
            {
                return playlists.Find(x => x.Name == name);
            }
        }

        public void LoadPlaylist(Playlist playlist)
        {
            LoadedPlaylist = playlist;
        }

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
            lock (this)
            {
                using (IUnitOfWork uow = DbFactory.GetUnitOfWork())
                {
                    uow.PlaylistRepository.UpdatePlaylist(playlist);
                    uow.SaveChanges();
                }
            }
        }

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

        public void RefreshPlaylists()
        {
            lock (this)
            {
                using (IUnitOfWork uow = DbFactory.GetUnitOfWork())
                    playlists = uow.PlaylistRepository.GetAllPlaylists();
            }
        }
    }
}
