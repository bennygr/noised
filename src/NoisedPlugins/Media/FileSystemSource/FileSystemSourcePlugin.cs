using System;
using System.Collections.Generic;
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
        #region Contructor

        /// <summary>
        ///		Constructor
        /// </summary>
        /// <param name="initalizer">initalizting data</param>        
        public FileSystemSourcePlugin(PluginInitializer initalizer) { }

        #endregion

        #region IDisposable

        public void Dispose() { }

        #endregion

        #region IPlugin

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

        public MediaItem GetItem(Uri uri)
        {
            //TODO: implement: Get the item from DB 
            return new MediaItem()
            {
                Uri = uri
            };
        }

        public IEnumerable<MediaItem> Search(string search)
        {
            //Just a test for now...
            return new List<MediaItem>()
            {
                new MediaItem()
                {
                    Uri= new Uri("file:///home/bgr/Musik/test.mp3")
                }
            };
        }

        #endregion
    };
}
