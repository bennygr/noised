using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using IrrKlang;
using Noised.Core.Media;
using Noised.Core.Plugins;
using Noised.Core.Plugins.Audio;
using Noised.Core.Resources;
using Noised.Logging;

namespace Noised.Plugins.Audio.IrrKlang
{
    /// <summary>
    /// Audio Playback via IrrKlang Framework
    /// </summary>
    public class NoisedIrrKlangPlugin : IAudioPlugin
    {
        // SoundEngine used by IrrKlang
        private readonly ISoundEngine engine;
        private readonly ILogging log;

        // Current Song
        private ISound currentPlayback;

        /// <summary>
        /// Audio Playback via IrrKlang Framework
        /// </summary>
        /// <param name="pluginInitializer">Noised PluginInitializer</param>
        public NoisedIrrKlangPlugin(PluginInitializer pluginInitializer)
        {
            log = pluginInitializer.Logging;

            log.Debug("Initialize IrrKlang plugin");

            log.Debug("Extract IrrKlang librabries");
            string directoryName = Path.GetDirectoryName(Assembly.GetEntryAssembly().CodeBase);
            if (directoryName != null)
            {
                string exeDir = directoryName.Replace("file:\\", string.Empty);
                ResourceExtractor.ExtractEmbeddedResource(exeDir, "Noised.Plugins.Audio.IrrKlang.Resources", new List<string> { "ikpFlac.dll", "ikpMP3.dll" });
            }
            else
                log.Error("Unable to extract IrrKlang libraries: unable to locate assembly.");

            log.Debug("Create IrrKlang SoundEngine");
            engine = new ISoundEngine();
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Führt anwendungsspezifische Aufgaben aus, die mit dem Freigeben, Zurückgeben oder Zurücksetzen von nicht verwalteten Ressourcen zusammenhängen.
        /// </summary>
        public void Dispose()
        {
            engine.Dispose();
            currentPlayback.Dispose();
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
            Play(item, 0);
        }

        /// <summary>
        ///		Play the specified item from a certain position
        /// </summary>
        /// <param name="item">The item to play</param>
        /// <param name="pos">The position from which to play playback in milliseconds</param>
        public void Play(MediaItem item, int pos)
        {
            log.Debug("Play -> " + item.Uri.AbsolutePath + " at position " + pos);
            currentPlayback = engine.Play2D(item.Uri.AbsolutePath);
            currentPlayback.PlayPosition = Convert.ToUInt32(pos);
            currentPlayback.setSoundStopEventReceiver(new SoundStopEventReceiver(InvokeOnSongFinished), item);
        }

        /// <summary>
        ///		Stops the playback
        /// </summary>
        public void Stop()
        {
            log.Debug("Stop");
            if (currentPlayback != null)
                currentPlayback.Stop();
        }

        /// <summary>
        ///		Pauses the playback
        /// </summary>
        public void Pause()
        {
            log.Debug("Pause");
            if (currentPlayback != null)
                currentPlayback.Paused = true;
        }

        /// <summary>
        ///		Resumes the playback
        /// </summary>
        public void Resume()
        {
            log.Debug("Resume");
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
                log.Debug("Set position " + value);
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
                log.Debug("Set volume " + value);
                if (currentPlayback != null)
                    currentPlayback.Volume = value;
            }
        }

        #endregion

        protected virtual void InvokeOnSongFinished(AudioEventArgs args)
        {
            if (SongFinished != null)
                SongFinished(this, args);
        }
    }
}
