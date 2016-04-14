using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using CoverArtArchive;
using MusicBrainz.Data;
using Noised.Core.Media;
using Noised.Core.Plugins;
using Noised.Core.Plugins.Media;
using Noised.Logging;

namespace Noised.Plugins.Media.MusicBrainzMetaFileScraper
{
    public class MusicBrainzScraper : IMetaFileScraper
    {
        private readonly ILogging log;

        public MusicBrainzScraper(PluginInitializer initializer)
        {
            log = initializer.Logging;

            log.Debug("Initialised " + Identifier);
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Führt anwendungsspezifische Aufgaben aus, die mit dem Freigeben, Zurückgeben oder Zurücksetzen von nicht verwalteten Ressourcen zusammenhängen.
        /// </summary>
        public void Dispose()
        { }

        #endregion

        #region Implementation of IMetaFileScraper

        /// <summary>
        /// Unique Identifier of the source
        /// </summary>
        public string Identifier
        {
            get
            {
                return "MusicBrainzScraper";
            }
        }

        /// <summary>
        /// Method that gets the album cover for an album name
        /// </summary>
        /// <param name="albumInfo">Name of the artist and the album</param>
        /// <returns>One or more album covers</returns>
        public IEnumerable<MetaFile> GetAlbumCover(ScraperAlbumInformation albumInfo)
        {
            log.Debug("Scraping MusicBrainz for " + albumInfo.Artist + "/" + albumInfo.Album);

            // Search for album cover in MusicBrainz service via album name and artist
            // current limit is 3 hits
            Release release = MusicBrainz.Search.Release(albumInfo.Album, null, null, albumInfo.Artist, null, null, null,
                null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,
                null, null, null, null, null, null, null, null, null, 3);

            if (release == null)
            {
                log.Debug("Nothing found in MusicBrainz for " + albumInfo.Artist + "/" + albumInfo.Album);
                yield break;
            }

            int i = 0;
            // we only consider hits that are "Official" and have a score of 100
            foreach (ReleaseData releaseData in release.Data.FindAll(x => x.Score == 100 && x.Status == "Official"))
            {
                foreach (Cover cover in CoverArtArchive.Release.Get.Cover(releaseData.Id).Images.FindAll(x => x.Front && !String.IsNullOrWhiteSpace(x.Image)))
                {
                    // determine here wether file is gallery, thumbnail etc. (https://stackoverflow.com/questions/123838/get-the-resolution-of-a-jpeg-image-using-c-sharp-and-the-net-environment)
                    //albumCovers.Add(new MetaFile(albumInfo.Artist, albumInfo.Album, MetaFileType.AlbumCover,
                    //    new Uri(cover.Image),
                    //    new WebClient().DownloadData(cover.Image), Path.GetExtension(cover.Image),
                    //    MetaFileCategory.Gallery, cover.Id + Path.GetExtension(cover.Image)));

                    yield return
                        new MetaFile(albumInfo.Artist, albumInfo.Album, MetaFileType.AlbumCover, new Uri(cover.Image),
                            new WebClient().DownloadData(cover.Image), Path.GetExtension(cover.Image),
                            MetaFileCategory.Gallery, cover.Id + Path.GetExtension(cover.Image));

                    i++;
                }
            }

            log.Debug("Found " + i + " album covers for " + albumInfo.Artist + "/" + albumInfo.Album);
        }

        /// <summary>
        /// Method that gets pictues of an artist
        /// </summary>
        /// <param name="artistName">The name of the artist</param>
        /// <returns>One or more pictues of an artist</returns>
        public IEnumerable<MetaFile> GetArtistPictures(string artistName)
        {
            return new List<MetaFile>();
        }

        #endregion
    }
}
