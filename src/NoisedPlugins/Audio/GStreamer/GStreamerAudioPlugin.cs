using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Noised.Core.Media;
using Noised.Core.Plugins;
using Noised.Core.Plugins.Audio;
using Noised.Logging;

namespace Noised.Plugins.Audio.GStreamer
{
    public class GStreamerAudioPlugin : IAudioPlugin
    {
        private MediaItem currentItem;
        private static GStreamerAudioPlugin plugin;
        private readonly AbstractGStreamerAccess gStreamerAccess;
        private ILogging logging;

        /// <summary>
        ///		Constructor
        /// </summary>
        public GStreamerAudioPlugin(PluginInitializer initalizer)
        {
            try
            {
                this.logging = initalizer.Logging;
#if UNIX
                { gStreamerAccess = new GStreamerAccessUnix(); }
#elif WINDOWS
                { gStreamerAccess = new GStreamerAccessWindows(); }
#else 
    #error "Unsupported Operating System"
#endif
                gStreamerAccess.AbsInitialize();
                gStreamerAccess.AbsSetSongFinishedCallback(Callback);
                plugin = this;
            }
            catch(Exception e)
            {
                if(this.logging != null)	
                {
                    this.logging.Error(e.ToString());
                }
            }
        }

        /// <summary>
        ///		Destructor
        /// </summary>
        ~GStreamerAudioPlugin()
        {
            Dispose(false);
        }

        #region Methods

        private void Callback()
        {
            try
            {
                //For some strange reason the this reference is NULL when get called from the
                //native code. Workaround saving this as static 
                plugin.OnSongFinished(plugin.currentItem);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        ///		Internal method to invoke the SongFinished event
        /// </summary>
        /// <param name="mediaItem">The item which has been finished</param>
        private void OnSongFinished(MediaItem mediaItem)
        {
            AudioEventHandler handler = SongFinished;
            if (handler != null)
            {
                handler(this,
                        new AudioEventArgs()
                        {
                            MediaItem = mediaItem
                        });
            }
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            //Telling the GC not to call the destructor when
            //collecting, because otherwise Dispose would 
            //be called twice
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Methods

        /// <summary>
        ///		Internal method to dispose plugin resources
        /// </summary>
        /// <param name="disposing">
        ///		True, if unmanaged reosources should be disposed in addition, 
        ///		False if only managed resources should be disposed
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //Disposing unmanaged resources
                gStreamerAccess.AbsFree();
            }

            //Disposing managed resources
            //...
        }

        #endregion

        #region IPlugin

        public String Name
        {
            get
            {
                return "GStreamerAudioPlugin";
            }
        }

        public String Description
        {
            get
            {
                return "The plugin allows sound output through the " +
                       "gstreamer library using gstreamer's high level playbin.";
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
                return DateTime.Parse("01.03.2015");
            }
        }

        #endregion

        #region IAudioPlugin

        public event AudioEventHandler SongFinished;

        public IEnumerable<string> SupportedProtocols
        {
            get
            {
                return new List<string> { "file://" };
            }
        }

        public void Play(MediaItem item)
        {
            Console.WriteLine("Playing a song..." + item.Uri);
            if (gStreamerAccess.AbsIsPlaying() ||
               gStreamerAccess.AbsIsPaused())
            {
                gStreamerAccess.AbsStop();
            }

            Task task = new Task(() => gStreamerAccess.AbsPlay(item.Uri.ToString()));
            currentItem = item;
            task.Start();
        }

        public void Play(MediaItem item, int pos)
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            gStreamerAccess.AbsStop();
        }
        public void Pause()
        {
            gStreamerAccess.AbsPause();
        }

        public void Resume()
        {
            gStreamerAccess.AbsResume();
        }

        public bool IsPlaying
        {
            get { throw new NotImplementedException(); }
        }

        public int Position
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int Length
        {
            get { throw new NotImplementedException(); }
        }

        public int Volume
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        #endregion
    };
}
