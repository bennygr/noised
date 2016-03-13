using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Noised.Core.DB;
using Noised.Core.IOC;
using Noised.Core.Plugins;
using Noised.Core.Plugins.Media;

namespace Noised.Core.Media
{
    public class MetaFileAccumulator : IMetaFileAccumulator
    {
        private readonly IPluginLoader pluginLoader;
        private readonly IDbFactory dbFactory;

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

        private static void WriteMetaFile(MetaFile metaFile)
        {
            IocContainer.Get<IMetaFileWriter>().WriteMetaFileToDisk(metaFile);
        }

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

            // Write AlbumCovers to disk/database
        }

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

            // Write ArtistPictures to disk/database
        }

        #endregion

        #region Implementation of IMetaFileAccumulator

        public void Refresh()
        {
            DistinctMetaDataCollection metaData = GetDistinctMetaData();
            RefreshArtistImages(metaData.Artists);
            RefreshAlbumCovers(metaData.Albums);
        }

        public void RefreshAsync()
        {
            DistinctMetaDataCollection metaData = GetDistinctMetaData();

            Parallel.ForEach(pluginLoader.GetPlugins<IMetaFileScraper>(), x => ProcessAsync(x, metaData));

            if (RefreshAsyncFinished != null)
                RefreshAsyncFinished();
        }

        public event Action RefreshAsyncFinished;

        #endregion

        private class DistinctMetaDataCollection
        {
            public List<string> Albums { get; private set; }
            public List<string> Artists { get; private set; }

            public DistinctMetaDataCollection(List<string> albums, List<string> artists)
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
