using System;
using System.Collections.Generic;
using System.IO;
using Noised.Core.Config;
using Noised.Core.Crypto;
using Noised.Core.IOC;
using Noised.Core.Media;
using Noised.Core.Plugins;
using Noised.Core.Plugins.Media;
using Noised.Plugins.FileSystemSource.DB;
using Noised.Plugins.FileSystemSource.DB.Sqlite;

namespace Noised.Plugins.Media.FileSystemSource
{
    /// <summary>
    ///		Local filesystem media source 
    /// </summary>
    public class FileSystemMediaSource : IPlugin, IMediaSource
    {
        private PluginInitializer core;
        public const string FILE_SYSTEM_MEDIA_SOURCE_NAME = "FILESYSTEM";

        #region Contructor

        /// <summary>
        ///		Constructor
        /// </summary>
        /// <param name="initalizer">initalizting data</param>        
        public FileSystemMediaSource(PluginInitializer initalizer)
        {
            this.core = initalizer;
            new FileSystemDBCreator().CreateOrUpdate();
        }

        #endregion

        /// <summary>
        ///		Internal method to read meta data from a media file
        /// </summary>
        private MetaData ReadMetaData(MediaItem mediaItem, FileInfo file)
        {
            TagLib.File tagFile = TagLib.File.Create(file.FullName);
            var tag = tagFile.Tag;
            return new MetaData(mediaItem)
            {
                Album = tag.Album,
                AlbumArtists = tag.AlbumArtists,
                Artists = tag.Performers,
                BeatsPerMinute = tag.BeatsPerMinute,
                Comment = tag.Comment,
                Composers = tag.Composers,
                Conductor = tag.Conductor,
                Copyright = tag.Copyright,
                Disc = tag.Disc,
                DiscCount = tag.DiscCount,
                Genres = tag.Genres,
                Grouping = tag.Grouping,
                Lyrics = tag.Lyrics,
                Title = tag.Title,
                TrackCount = tag.TrackCount,
                TrackNumber = tag.Track,
                Year = tag.Year
            };
        }

        /// <summary>
        ///		Internal method to load a file from disk
        /// </summary>
        private void RefreshFile(FileInfo file)
        {
			var logger = core.Logging;
            using (var unitOfWork = new SqliteFileSystemUnitOfWork())
            {
                var checksum = IocContainer.Get<IChecksum>().CalculateChecksum(file);
                var uri = new Uri("file://" + file.FullName);

                //Check if we already know the media item
				bool updating = false;
                var existingItem = unitOfWork.MediaItemRepository.GetByUri(uri);
                if (existingItem != null && existingItem.Checksum != checksum)
                {
					logger.Debug(String.Format("MediaItem {0} changed",file.FullName));
                    //Delete if the checksum of the file has been changed 
                    unitOfWork.MediaItemRepository.Delete(existingItem);
                    existingItem = null;
					updating = true;
                }

                if (existingItem == null)
                {
                    var mediaItem = new MediaItem(uri, checksum);
                    mediaItem.MetaData = ReadMetaData(mediaItem, file);
                    unitOfWork.MediaItemRepository.Create(mediaItem);
                    unitOfWork.SaveChanges();
					if(updating)
					{
						logger.Debug(String.Format("Updated existing MediaItem {0}",file.FullName));
					}				
					else
					{
						logger.Debug(String.Format("Added new MediaItem {0}",file.FullName));
					}
				}
            }
        }

        /// <summary>
        ///		Internal method to load media items from a given directory
        /// </summary>
        private void RefreshDirectory(DirectoryInfo directory, bool recursive)
        {
            if (directory.Exists)
            {

                //Checking all files in the directory
                foreach (var fileInfo in directory.GetFiles())
                {
                    if (SupportedFileTypes.IsFileSupported(fileInfo))
                    {
                        try
                        {
                            RefreshFile(fileInfo);
                        }
                        catch (Exception e)
                        {
                            core.Logging.Error(String.Format("Could not refresh media file {0}: {1}",
                                    fileInfo.FullName,
                                    e.Message));
                            core.Logging.Error(e.StackTrace);
                        }
                    }
                }

                //Checking sub directories
                if (recursive)
                {
                    foreach (var subDirectory in directory.GetDirectories())
                    {
                        RefreshDirectory(subDirectory, recursive: true);
                    }
                }
            }
            else
            {
                core.Logging.Error(String.Format("Directory {0} does not exist. Skipping.", directory));
            }
        }

        #region IDisposable

        public void Dispose()
        {
        }

        #endregion

        #region IPlugin

        public Guid Guid
        {
            get { return Guid.Parse("17abe4da-0480-4481-9aee-2baf362dfe50"); }
        }

        public String Name
        {
            get
            {
                return "FileSystemSourcePlugin";
            }
        }

        public String Description
        {
            get
            {
                return "A media source from a local filesystem.";
            }
        }

        public String AuthorName
        {
            get
            {
                return "Benjamin Grüdelbach";
            }
        }

        public String AuthorContact
        {
            get
            {
                return "nocontact@availlable.de";
            }
        }

        public Version Version
        {
            get
            {
                return new Version(1, 0);
            }
        }

        public DateTime CreationDate
        {
            get
            {
                return DateTime.Parse("21.03.2015");
            }
        }


        #endregion

        #region IMediaSource

		public string Identifier
		{
			get
			{
				return FILE_SYSTEM_MEDIA_SOURCE_NAME;
			}
		}
        public void Refresh()
        {
			var logger = core.Logging;
            //loading all media files from a specified directory
            var config = core.Get<IConfig>();
            string directoryList = config.GetProperty(FileSystemSourceProperties.Directories);
            if (!string.IsNullOrEmpty(directoryList))
            {
                core.Logging.Debug("Parsing music directories...");
                var directories = directoryList.Split(FileSystemSourceProperties.SplitCharacter);
                foreach (var directory in directories)
                {
					logger.Info(String.Format("Refreshing Music directory {0}...",directory));
                    RefreshDirectory(new DirectoryInfo(directory), recursive: true);
					logger.Info(String.Format("Refreshed Directory {0}",directory));
                }
            }
            else
            {
                logger.Warning("No music directories found.");
				logger.Warning("noised.plugins.media.filesystemsource.directories is not defined in filesystemsource.nconfig");
            }
        }

        public MediaItem Get(Uri uri)
        {
            return null;
        }

        public IEnumerable<MediaSourceSearchResult> Search(string pattern)
        {
            //Just a test for now...
            return new List<MediaSourceSearchResult>
            {
                new MediaSourceSearchResult(FILE_SYSTEM_MEDIA_SOURCE_NAME,
                    new MediaItem(new Uri(@"file:C:\Users\sbingel.DATASEC2003\Downloads\irrKlang-32bit-1.5.0\irrKlang-1.5.0\media\ophelia.mp3"), null))

            };
        }

        #endregion
    };
}
