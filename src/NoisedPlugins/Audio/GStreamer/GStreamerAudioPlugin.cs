using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Noised.Core.Plugins;
using Noised.Core.Plugins.Audio;
using Noised.Core.Media;

namespace Noised.Plugins.Audio.GStreamer
{
	public class GStreamerAudioPlugin : IAudioPlugin 
	{
		/// <summary>
		///		Constructor
		/// </summary>
		public GStreamerAudioPlugin(PluginInitializer initalizer)
		{
			GStreamerAccessUnix.Initialize();
		}

		/// <summary>
		///		Destructor
		/// </summary>
		~GStreamerAudioPlugin()
		{
			Dispose(false);
		}

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
			if(disposing)
			{
				//Disposing unmanaged resources
				GStreamerAccessUnix.Free();	
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
		
		#region IAudioPlugin
		
		public IEnumerable<string> SupportedProtocols
		{
			get
			{
				return new List<string>(){"file://"};
			}
		}

		public void Play(MediaItem item)
		{
			Console.WriteLine("Playing a song...");
			if(GStreamerAccessUnix.IsPlaying() || 
			   GStreamerAccessUnix.IsPaused())
			{
				GStreamerAccessUnix.Stop();
			}

			Task task = new Task( () => GStreamerAccessUnix.Play(item.Uri.ToString()) );
			task.Start();
		}

		public void Play(MediaItem item, int pos)
		{
			throw new NotImplementedException();
		}

		public void Stop()
		{
			GStreamerAccessUnix.Stop();
		}
		public void Pause()
		{
			GStreamerAccessUnix.Pause();
		}

		public void Resume()
		{
			GStreamerAccessUnix.Resume();
		}

		public bool IsPlaying
		{
			get{ throw new NotImplementedException(); }
		}

		public int Position
		{
			get{ throw new NotImplementedException();}
			set{ throw new NotImplementedException();}
		}

		public int Length
		{
			get{ throw new NotImplementedException();}
		}

		public int Volume
		{
			get{ throw new NotImplementedException();}
			set{ throw new NotImplementedException();}
		}
		
		#endregion
	};
}
