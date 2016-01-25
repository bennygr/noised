using System;
using IrrKlang;
using Noised.Core.Media;
using Noised.Core.Plugins.Audio;

namespace Noised.Plugins.Audio.IrrKlang
{
    public class SoundStopEventReceiver : ISoundStopEventReceiver
    {
        private readonly Action<AudioEventArgs> invokeOnSongFinished;

        public SoundStopEventReceiver(Action<AudioEventArgs> invokeOnSongFinished)
        {
            this.invokeOnSongFinished = invokeOnSongFinished;
        }

        #region Implementation of ISoundStopEventReceiver

        public void OnSoundStopped(ISound sound, StopEventCause reason, object userData)
        {
            invokeOnSongFinished(new AudioEventArgs { MediaItem = (MediaItem)userData });
        }

        #endregion
    }
}
