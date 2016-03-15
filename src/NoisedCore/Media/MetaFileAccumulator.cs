using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Noised.Core.DB;
using Noised.Core.IOC;
using Noised.Core.Plugins;
using Noised.Core.Plugins.Media;

namespace Noised.Core.Media
{
    /// <summary>
    /// Access to all MetaFiles sources
    /// </summary>
    public class MetaFileAccumulator : IMetaFileAccumulator
    {
        private readonly IPluginLoader pluginLoader;
        private readonly IDbFactory dbFactory;

        /// <summary>
        /// Access to all MetaFiles sources
        /// </summary>
        /// <param name="pluginLoader">PluginLoader</param>
        /// <param name="dbFactory">Database factory</param>
        public MetaFileAccumulator(IPluginLoader pluginLoader, IDbFactory dbFactory)
        {
            if (pluginLoader == null)
                throw new ArgumentNullException("pluginLoader");
            if (dbFactory == null)
                throw new ArgumentNullException("dbFactory");

            this.pluginLoader = pluginLoader;
            this.dbFactory = dbFactory;
        }

        #region Methods

        /// <summary>
        /// Internal method to refresh a IMetaFileScraper asynchronously (it's called asynchronously)
        /// </summary>
        /// <param name="scraper">The scraper which is about be refreshed</param>
        /// <param name="metaData">The input for the scraper</param>
        private void ProcessAsync(IMetaFileScraper scraper, DistinctMetaDataCollection metaData)
        {
            List<MetaFile> albumCovers = new List<MetaFile>();
            List<MetaFile> artistPictures = new List<MetaFile>();

            foreach (string album in metaData.Albums)
                albumCovers.AddRange(scraper.GetAlbumCover(album));

            foreach (string artist in metaData.Artists)
                artistPictures.AddRange(scraper.GetArtistPictures(artist));

            using (IUnitOfWork uow = dbFactory.GetUnitOfWork())
            {
                foreach (MetaFile artistPicture in artistPictures)
                {
                    WriteMetaFile(artistPicture);
                    uow.MetaFileRepository.CreateMetaFile(artistPicture);
                }

                foreach (MetaFile albumCover in albumCovers)
                {
                    WriteMetaFile(albumCover);
                    uow.MetaFileRepository.CreateMetaFile(albumCover);
                }

                uow.SaveChanges();
            }
        }

        /// <summary>
        /// Methof to write a MetaFile to the disk
        /// </summary>
        /// <param name="metaFile">The MetaFile</param>
        private static void WriteMetaFile(MetaFile metaFile)
        {
            IocContainer.Get<IMetaFileWriter>().WriteMetaFileToDisk(metaFile);
        }

        /// <summary>
        /// Gets a distinct collection of all MetaData (Artist, Album)
        /// </summary>
        /// <returns>A distinct collection of all MetaData (Artist, Album)</returns>
        private static DistinctMetaDataCollection GetDistinctMetaData()
        {
            List<string> artists = new List<string>();
            List<string> albums = new List<string>();
            foreach (MediaSourceSearchResult mediaSourceSearchResult in IocContainer.Get<IMediaSourceAccumulator>().Search("*"))
            {
                foreach (MediaItem mediaItem in mediaSourceSearchResult.MediaItems)
                {
                    foreach (string artist in mediaItem.MetaData.Artists)
                        artists.Add(artist);
                    foreach (string albumArtist in mediaItem.MetaData.AlbumArtists)
                        artists.Add(albumArtist);
                    albums.Add(mediaItem.MetaData.Album);
                }
            }

            return new DistinctMetaDataCollection(albums, artists);
        }

        /// <summary>
        /// Refreshes the albums Covers over all Scrapers
        /// </summary>
        /// <param name="albums">List of all albums that will be refreshed</param>
        private void RefreshAlbumCovers(List<string> albums)
        {
            List<MetaFile> albumCovers = new List<MetaFile>();

            foreach (IMetaFileScraper scraper in pluginLoader.GetPlugins<IMetaFileScraper>())
            {
                foreach (string album in albums)
                {
                    albumCovers.AddRange(scraper.GetAlbumCover(album));
                }
            }

            foreach (MetaFile albumCover in albumCovers)
                WriteMetaFile(albumCover);
        }

        /// <summary>
        /// Refreshes the artist images
        /// </summary>
        /// <param name="artists">List of all artists for which the scraper should load new images</param>
        private void RefreshArtistImages(List<string> artists)
        {
            List<MetaFile> artistPictures = new List<MetaFile>();

            foreach (IMetaFileScraper scraper in pluginLoader.GetPlugins<IMetaFileScraper>())
            {
                foreach (string artist in artists)
                {
                    artistPictures.AddRange(scraper.GetAlbumCover(artist));
                }
            }

            foreach (MetaFile artistPicture in artistPictures)
                WriteMetaFile(artistPicture);
        }

        #endregion

        #region Implementation of IMetaFileAccumulator

        /// <summary>
        /// Refreshs all MetaFiles from alle MetaFile sources
        /// </summary>
        public void Refresh()
        {
            DistinctMetaDataCollection metaData = GetDistinctMetaData();
            RefreshArtistImages(metaData.Artists);
            RefreshAlbumCovers(metaData.Albums);
        }

        /// <summary>
        /// Refreshs all MetaFiles from alle MetaFile sources asynchronous
        /// </summary>
        public void RefreshAsync()
        {
            DistinctMetaDataCollection metaData = GetDistinctMetaData();

            Parallel.ForEach(pluginLoader.GetPlugins<IMetaFileScraper>(), x => ProcessAsync(x, metaData));

            if (RefreshAsyncFinished != null)
                RefreshAsyncFinished();
        }

        /// <summary>
        /// A calback that fires when the asynchronous refresh is finished
        /// </summary>
        public event Action RefreshAsyncFinished;

        #endregion

        /// <summary>
        /// A Space to store Lists of MetaData to process by Scrapers
        /// </summary>
        private class DistinctMetaDataCollection
        {
            internal List<string> Albums { get; private set; }
            internal List<string> Artists { get; private set; }

            internal DistinctMetaDataCollection(List<string> albums, List<string> artists)
            {
                if (albums == null)
                    throw new ArgumentNullException("albums");
                if (artists == null)
                    throw new ArgumentNullException("artists");
                Albums = albums;
                Artists = artists;
            }
        }
    }
}
