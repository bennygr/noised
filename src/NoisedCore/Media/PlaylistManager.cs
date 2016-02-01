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
            using (IUnitOfWork unitOfWork = IocContainer.Get<IUnitOfWork>())
                playlists = unitOfWork.PlaylistRepository.GetAllPlaylists();
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

            using (IUnitOfWork unitOfWork = IocContainer.Get<IUnitOfWork>())
            {
                unitOfWork.PlaylistRepository.CreatePlaylist(playlist);
                unitOfWork.SaveChanges();
            }

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
            using (IUnitOfWork unitOfWork = IocContainer.Get<IUnitOfWork>())
            {
                unitOfWork.PlaylistRepository.DeletePlaylist(playlist);
                unitOfWork.SaveChanges();
            }
        }

        /// <summary>
        /// Saves a Playlist to the configured medium
        /// </summary>
        /// <param name="playlist"></param>
        public void SavePlaylist(Playlist playlist)
        {
            using (IUnitOfWork unitOfWork = IocContainer.Get<IUnitOfWork>())
            {
                unitOfWork.PlaylistRepository.UpdatePlaylist(playlist);
                unitOfWork.SaveChanges();
            }
        }
    }
}
