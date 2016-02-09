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
        private IUnitOfWork uow;

        public ReadOnlyCollection<Playlist> Playlists
        {
            get
            {
                return new ReadOnlyCollection<Playlist>(playlists);
            }
        }

        public Playlist LoadedPlaylist { get; private set; }

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

                //using (uow)
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
                //using (uow)
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
                //using (uow)
                {
                    uow.PlaylistRepository.UpdatePlaylist(playlist);
                    uow.SaveChanges();
                }
            }
        }

        public void SetUnitOfWork(IUnitOfWork unitOfWork)
        {
            uow = unitOfWork;
        }

        public void RefreshPlaylists()
        {
            //using(uow)
                playlists = uow.PlaylistRepository.GetAllPlaylists();
        }
    }
}
