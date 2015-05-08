using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Noised.Core.Media;
using Noised.Core.Plugins;
using Noised.Core.Plugins.Audio;

namespace Noised.Plugins.Audio.Dummy
{
	/// <summary>
	///		A dummy audio plugin which does not play any sound
	/// </summary>
	/// <remarks> This is for testing purpose</remarks>
	public class DummyAudioPlugin  : IAudioPlugin
	{
		private bool isPlaying;

		#region Constructor
		
		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="pluginInitializer">initalizer</param>
		public DummyAudioPlugin(PluginInitializer pluginInitializer) { }
		
		#endregion

		#region IDisposable
		
		public void Dispose() { }
		
		#endregion

		#region IPlugin
		
		public String Name
		{
			get
			{
				return "DummyAudioPlugin";
			}
		}

		public String Description
		{
			get
			{
				return "Just a dummy audio plugin which does not " + 
					   "play any sound(for testing purpose only)";
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
				return new Version(1,0);
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

		#region IAudioPlugin

		public event AudioEventHandler SongFinished;

		public IEnumerable<string> SupportedProtocols
		{
			get
			{
				return new List<string>(){"file://"};
			}
		}
		
		public void Play(MediaItem item)
		{
			Play(item,0);
		}

		public void Play(MediaItem item, int pos)
		{
			isPlaying = true;
			Console.WriteLine( String.Format("Playing song {0} from position {1}",
											 item.Uri.ToString(), 
											 pos));
			Thread.Sleep(3000);
			OnSongFinished(item);
		}

		public void Stop()
		{
			isPlaying = false;
			Console.WriteLine("Playback stopped");
		}
		public void Pause()
		{
			Console.WriteLine("Playback paused");
		}

		public void Resume()
		{
			Console.WriteLine("Playback resumed");
		}

		public bool IsPlaying
		{
			get{ return isPlaying;}
		}

		public int Position{get;set;}

		public int Length
		{
			get{ return 100;}
		}

		public int Volume { get;set; }
		
		#endregion
	};
}
