using System;
using System.Collections.Generic;
using System.IO;
using Noised.Core.Config;
using Noised.Core.Crypto;
using Noised.Core.DB;
using Noised.Core.IOC;
using Noised.Core.Media;
using Noised.Core.Plugins;
using Noised.Core.Plugins.Media;

namespace Noised.Plugins.Media.FileSystemSource
{
    /// <summary>
    ///		Local filesystem media source 
    /// </summary>
    public class FileSystemSourcePlugin : IPlugin, IMediaSource
    {
        private PluginInitializer core;

        #region Contructor

        /// <summary>
        ///		Constructor
        /// </summary>
        /// <param name="initalizer">initalizting data</param>        
        public FileSystemSourcePlugin(PluginInitializer initalizer)
        {
            this.core = initalizer;
        }

        #endregion

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

		private void RefreshFile(FileInfo file)
		{
			using(var unitOfWork = IocContainer.Get<IUnitOfWork>())
			{
				var checksum = IocContainer.Get<IChecksum>().CalculateChecksum(file);
				var mediaItem = new MediaItem(new Uri("file://" + file.FullName),checksum);
				mediaItem.MetaData = ReadMetaData(mediaItem,file);
				unitOfWork.MediaItemRepository.Create(mediaItem);
				unitOfWork.SaveChanges();
			}
		}

        /// <summary>
        ///		Internal method to load media items from a given directory
        /// </summary>
        private void RefreshDirectory(DirectoryInfo directory, bool recursive)
        {
            core.Logging.Debug(String.Format("Looking for music in directory {0}", directory));
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
						catch(Exception e)
						{
							core.Logging.Error(String.Format("Could not refresh media file {0}",fileInfo.FullName));
							core.Logging.Error(e.Message);
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

        public void Refresh()
        {
            //loading all media files from a specified directory
            var config = core.Get<IConfig>();
            string directoryList = config.GetProperty(FileSystemSourceProperties.Directories);
            if (!string.IsNullOrEmpty(directoryList))
            {
                core.Logging.Debug("Parsing music directories...");
                var directories = directoryList.Split(FileSystemSourceProperties.SplitCharacter);
                foreach (var directory in directories)
                {
                    RefreshDirectory(new DirectoryInfo(directory), recursive: true);
                }
            }
            else
            {
                core.Logging.Warning("No music directories found: noised.plugins.media.filesystemsource.directories is not defined in filesystemsource.nconfig");
            }
        }

        public MediaItem GetItem(Uri uri)
        {
            //TODO: implement: Get the item from DB 
            return new MediaItem(uri,null);
        }

        public IEnumerable<MediaItem> Search(string search)
        {
            //Just a test for now...
            return new List<MediaItem>
            {
                new MediaItem(new Uri(@"file:C:\Users\sbingel.DATASEC2003\Downloads\irrKlang-32bit-1.5.0\irrKlang-1.5.0\media\ophelia.mp3"),null) 
            };
        }

        #endregion
    };
}
