using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
        /// <param name="metaDataCollection">The input for the scraper</param>
        private void RefreshAsync(IMetaFileScraper scraper, DistinctMetaDataCollection metaDataCollection)
        {
            using (IUnitOfWork uow = dbFactory.GetUnitOfWork())
            {
                // Get all AlbumCovers from IMetaFileScraper
                foreach (ScraperAlbumInformation album in metaDataCollection.Albums)
                {
                    foreach (MetaFile mf in scraper.GetAlbumCover(album))
                    {
                        WriteMetaFile(mf);
                        uow.MetaFileRepository.CreateMetaFile(mf);
                    }
                }

                // Get all Artistpictures from IMetaFileScraper
                foreach (string artist in metaDataCollection.Artists)
                {
                    foreach (MetaFile mf in scraper.GetArtistPictures(artist))
                    {
                        WriteMetaFile(mf);
                        uow.MetaFileRepository.CreateMetaFile(mf);
                    }
                }
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
            ConcurrentBag<string> artists = new ConcurrentBag<string>();
            ConcurrentBag<ScraperAlbumInformation> albums = new ConcurrentBag<ScraperAlbumInformation>();

            // Search every MediaItem in every MediaSource and iterate over the results.
            // Because of possible time intensive searches we iterate parallel
            Parallel.ForEach(IocContainer.Get<IMediaSourceAccumulator>().Search("*"), searchResult =>
            {
                // iterate over all MediaItems of all serachresults
                foreach (MediaItem mediaItem in searchResult.MediaItems)
                {
                    // iterate over all Artists and all AlbumArtists of the MediaItem
                    foreach (string artist in mediaItem.MetaData.Artists.Concat(mediaItem.MetaData.AlbumArtists))
                    {
                        // Add every artist to artists
                        artists.Add(artist);

                        // if combination is already known just continue
                        if (albums.Any(y => y.Artist == artist && y.Album == mediaItem.MetaData.Album))
                            continue;

                        // otherwise create new ScraperAlbumInformation from artist and Album
                        ScraperAlbumInformation albumInfo = new ScraperAlbumInformation(artist, mediaItem.MetaData.Album);

                        // and add it to albums
                        albums.Add(albumInfo);
                    }
                }
            });

            // Return collection of all Album/Artists combination and all Artists
            return new DistinctMetaDataCollection(albums.ToList(), artists.Distinct().ToList());
        }

        /// <summary>
        /// Refreshes the albums Covers over all Scrapers
        /// </summary>
        /// <param name="albums">List of all albums that will be refreshed</param>
        private void RefreshAlbumCovers(List<ScraperAlbumInformation> albums)
        {
            List<MetaFile> albumCovers = new List<MetaFile>();

            foreach (IMetaFileScraper scraper in pluginLoader.GetPlugins<IMetaFileScraper>())
            {
                foreach (ScraperAlbumInformation album in albums)
                {
                    albumCovers.AddRange(scraper.GetAlbumCover(new ScraperAlbumInformation(album.Artist, album.Album)));
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
                    artistPictures.AddRange(scraper.GetArtistPictures(artist));
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
            Task.Run(
                () =>
                {
                    // Get Collection of all Album/Artist combinations and all Artists
                    DistinctMetaDataCollection metaData = GetDistinctMetaData();
                    // Iterate over all registered IMetaFileScapers and execute them asynchronously
                    Parallel.ForEach(pluginLoader.GetPlugins<IMetaFileScraper>(), x => RefreshAsync(x, metaData));
                    if (RefreshAsyncFinished != null)
                        RefreshAsyncFinished();
                });
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
            internal List<ScraperAlbumInformation> Albums { get; private set; }
            internal List<string> Artists { get; private set; }

            internal DistinctMetaDataCollection(List<ScraperAlbumInformation> albums, List<string> artists)
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
