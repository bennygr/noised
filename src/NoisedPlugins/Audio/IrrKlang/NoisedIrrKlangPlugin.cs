using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Noised.Core.Media;
using Noised.Core.Plugins;
using Noised.Core.Plugins.Audio;

namespace IrrKlang
{
    public class NoisedIrrKlangPlugin : IAudioPlugin
    {
        private readonly ISoundEngine engine;

        private ISound currentPlayback;

        public NoisedIrrKlangPlugin(PluginInitializer pluginInitializer)
        {
            string exeDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().CodeBase).Replace("file:\\", string.Empty);
            ExtractEmbeddedResource(exeDir, "IrrKlang.Resources",
                new List<string> { "ikpFlac.dll", "ikpMP3.dll" });

            engine = new ISoundEngine();
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Führt anwendungsspezifische Aufgaben aus, die mit dem Freigeben, Zurückgeben oder Zurücksetzen von nicht verwalteten Ressourcen zusammenhängen.
        /// </summary>
        public void Dispose()
        { }

        #endregion

        #region Implementation of IPlugin

        /// <summary>
        ///		The name of the plugin
        /// </summary>
        public string Name
        {
            get
            {
                return "IrrKlang Audio Plugin";
            }
        }

        /// <summary>
        ///		The description of the plugin
        /// </summary>
        public string Description
        {
            get
            {
                return "Audio Plugin using irrKlang.NET4 Library for audio playback";
            }
        }

        /// <summary>
        ///		The name of the author
        /// </summary>
        public string AuthorName
        {
            get
            {
                return "sebingel";
            }
        }

        /// <summary>
        ///		The contact of the author
        /// </summary>
        public string AuthorContact
        {
            get
            {
                return "sebingel+noisedcontact@googlemail.com";
            }
        }

        /// <summary>
        ///		The version of the plugin
        /// </summary>
        public Version Version
        {
            get
            {
                return new Version(1, 0, 0, 0);
            }
        }

        /// <summary>
        ///		The creation date of the plugin		
        /// </summary>
        public DateTime CreationDate
        {
            get
            {
                return new DateTime(2015, 10, 18);
            }
        }

        #endregion

        #region Implementation of IAudioPlugin

        public event AudioEventHandler SongFinished;

        /// <summary>
        ///		A list of supported protocols to play
        /// </summary>
        /// <remarks>
        ///		For example file://, spotify://, etc
        /// </remarks>
        public IEnumerable<string> SupportedProtocols
        {
            get
            {
                return new List<string>
                {
                    "file://"
                };
            }
        }

        /// <summary>
        ///		Plays the specified item 
        /// </summary>
        /// <param name="item">The item to play</param>
        public void Play(MediaItem item)
        {
            currentPlayback = engine.Play2D(item.Uri.AbsolutePath);
        }

        /// <summary>
        ///		Play the specified item from a certain position
        /// </summary>
        /// <param name="item">The item to play</param>
        /// <param name="pos">The position from which to play playback in milliseconds</param>
        public void Play(MediaItem item, int pos)
        {
            currentPlayback = engine.Play2D(item.Uri.AbsolutePath);
            currentPlayback.PlayPosition = Convert.ToUInt32(pos);
        }

        /// <summary>
        ///		Stops the playback
        /// </summary>
        public void Stop()
        {
            if (currentPlayback != null)
                currentPlayback.Stop();
        }

        /// <summary>
        ///		Pauses the playback
        /// </summary>
        public void Pause()
        {
            if (currentPlayback != null)
                currentPlayback.Paused = true;
        }

        /// <summary>
        ///		Resumes the playback
        /// </summary>
        public void Resume()
        {
            if (currentPlayback != null)
                currentPlayback.Paused = false;
        }

        /// <summary>
        ///		Whether the player is playing or not
        /// </summary>
        public bool IsPlaying
        {
            get
            {
                if (currentPlayback != null)
                    return currentPlayback.Paused;
                return false;
            }
        }

        /// <summary>
        ///		The current position of the playback in milliseconds
        /// </summary>
        public int Position
        {
            get
            {
                if (currentPlayback != null)
                    return Convert.ToInt32(currentPlayback.PlayPosition);
                return -1;
            }
            set
            {
                if (currentPlayback != null)
                    currentPlayback.PlayPosition = Convert.ToUInt32(value);
            }
        }

        /// <summary>
        ///		The length of the current playback in milliseconds
        /// </summary>
        public int Length
        {
            get
            {
                if (currentPlayback != null)
                    return Convert.ToInt32(currentPlayback.PlayLength);
                return -1;
            }
        }

        /// <summary>
        ///		The current volume in percent
        /// </summary>
        public int Volume
        {
            get
            {
                if (currentPlayback != null)
                    return Convert.ToInt32(currentPlayback.Volume);
                return -1;
            }
            set
            {
                if (currentPlayback != null)
                    currentPlayback.Volume = value;
            }
        }

        #endregion

        private static void ExtractEmbeddedResource(string outputDir, string resourceLocation, List<string> files)
        {
            foreach (string file in files)
            {
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceLocation + @"." + file))
                {
                    string path = Path.Combine(outputDir, file);

                    if (File.Exists(path))
                        continue;

                    using (FileStream fileStream = new FileStream(path, FileMode.Create))
                    {
                        for (int i = 0; i < stream.Length; i++)
                            fileStream.WriteByte((byte)stream.ReadByte());
                        fileStream.Close();
                    }
                }
            }
        }
    }
}
