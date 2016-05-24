using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Noised.Core.DB;
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
        private readonly IMetaFileWriter metaFileWriter;
        private readonly IMediaSourceAccumulator mediaSourceAccumulator;
        // TODO: remove this locking and make underlying classes threadsafe
        private readonly Object lockUnitOfWork = new Object();

        /// <summary>
        /// Access to all MetaFiles sources
        /// </summary>
        /// <param name="pluginLoader">PluginLoader</param>
        /// <param name="dbFactory">Database factory</param>
        /// <param name="metaFileWriter">MetaFileWriter</param>
        /// <param name="mediaSourceAccumulator">MediaSourceAccumulator</param>
        public MetaFileAccumulator(IPluginLoader pluginLoader, IDbFactory dbFactory, IMetaFileWriter metaFileWriter, IMediaSourceAccumulator mediaSourceAccumulator)
        {
            if (pluginLoader == null)
                throw new ArgumentNullException("pluginLoader");
            if (dbFactory == null)
                throw new ArgumentNullException("dbFactory");

            this.pluginLoader = pluginLoader;
            this.dbFactory = dbFactory;
            this.metaFileWriter = metaFileWriter;
            this.mediaSourceAccumulator = mediaSourceAccumulator;
        }

        #region Methods

        /// <summary>
        /// Internal method to refresh a IMetaFileScraper asynchronously (it's called asynchronously)
        /// </summary>
        /// <param name="scraper">The scraper which is about be refreshed</param>
        /// <param name="metaDataCollection">The input for the scraper</param>
        private void RefreshAsyncInternal(IMetaFileScraper scraper, DistinctMetaDataCollection metaDataCollection)
        {
            // Get all AlbumCovers from IMetaFileScraper
            RefreshAlbumCovers(scraper, metaDataCollection.Albums, metaFileWriter);

            // Get all Artistpictures from IMetaFileScraper
            RefreshArtistImages(scraper, metaDataCollection.Artists, metaFileWriter);
        }

        /// <summary>
        /// Gets a distinct collection of all MetaData (Artist, Album)
        /// </summary>
        /// <returns>A distinct collection of all MetaData (Artist, Album)</returns>
        private DistinctMetaDataCollection GetDistinctMetaData()
        {
            ConcurrentBag<string> artists = new ConcurrentBag<string>();
            ConcurrentBag<ScraperAlbumInformation> albums = new ConcurrentBag<ScraperAlbumInformation>();

            // Search every MediaItem in every MediaSource and iterate over the results.
            // Because of possible time intensive searches we iterate parallel
            Parallel.ForEach(mediaSourceAccumulator.Search("*"), searchResult =>
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
        /// <param name="scraper">IMetaFileScraper to get the MetaFiles</param>
        /// <param name="albums">List of all albums that will be refreshed</param>
        /// <param name="mfw">IMetaFileWriter to save the MetaFile</param>
        private void RefreshAlbumCovers(IMetaFileScraper scraper, List<ScraperAlbumInformation> albums, IMetaFileWriter mfw)
        {
            // Get all Artistpictures from IMetaFileScraper
            foreach (ScraperAlbumInformation album in albums)
            {
                using (var uow = dbFactory.GetUnitOfWork())
                {
                    if (uow.MetaFileRepository.GetMetaFiles(album.Artist, album.Album).Any())
                        return;
                }

                foreach (MetaFile mf in scraper.GetAlbumCover(album))
                {
                    mfw.WriteMetaFileToDisk(mf);
                    // TODO: remove this locking and make underlying classes threadsafe
                    lock (lockUnitOfWork)
                    {
                        using (IUnitOfWork uow = dbFactory.GetUnitOfWork())
                        {
                            uow.MetaFileRepository.CreateMetaFile(mf);
                            uow.SaveChanges();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Refreshes the artist images
        /// </summary>
        /// <param name="scraper">IMetaFileScraper to get the MetaFiles</param>
        /// <param name="artists">List of all artists for which the scraper should load new images</param>
        /// <param name="mfw">IMetaFileWriter to save the MetaFile</param>
        private void RefreshArtistImages(IMetaFileScraper scraper, List<string> artists, IMetaFileWriter mfw)
        {
            // Get all Artistpictures from IMetaFileScraper
            foreach (string artist in artists)
            {
                foreach (MetaFile mf in scraper.GetArtistPictures(artist))
                {
                    mfw.WriteMetaFileToDisk(mf);
                    // TODO: remove this locking and make underlying classes threadsafe
                    lock (lockUnitOfWork)
                    {
                        using (IUnitOfWork uow = dbFactory.GetUnitOfWork())
                        {
                            uow.MetaFileRepository.CreateMetaFile(mf);
                            uow.SaveChanges();
                        }
                    }
                }
            }
        }

        #endregion

        #region Implementation of IMetaFileAccumulator

        /// <summary>
        /// Refreshs all MetaFiles from alle MetaFile sources
        /// </summary>
        public void Refresh()
        {
            // Get Collection of all Album/Artist combinations and all Artists
            DistinctMetaDataCollection metaData = GetDistinctMetaData();

            foreach (IMetaFileScraper scraper in pluginLoader.GetPlugins<IMetaFileScraper>())
            {
                RefreshArtistImages(scraper, metaData.Artists, metaFileWriter);
                RefreshAlbumCovers(scraper, metaData.Albums, metaFileWriter);
            }
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
                    Parallel.ForEach(pluginLoader.GetPlugins<IMetaFileScraper>(), x => RefreshAsyncInternal(x, metaData));
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

            /// <summary>
            /// A Space to store Lists of MetaData to process by Scrapers
            /// </summary>
            /// <param name="albums">A List of Albums</param>
            /// <param name="artists">A List of Artists (names)</param>
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
