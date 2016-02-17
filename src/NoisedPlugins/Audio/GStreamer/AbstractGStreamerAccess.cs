using System;
using System.Runtime.InteropServices;

namespace Noised.Plugins.Audio.GStreamer
{
    abstract internal class AbstractGStreamerAccess
    {
        public delegate void SongFinishedCallback();

        abstract internal void AbsInitialize();

        abstract internal void AbsFree();

        abstract internal void AbsPlay(String uri);

        abstract internal void AbsStop();

        abstract internal void AbsPause();

        abstract internal void AbsResume();

        abstract internal bool AbsIsPlaying();

        abstract internal bool AbsIsPaused();

        abstract internal void AbsSetSongFinishedCallback([MarshalAs(UnmanagedType.FunctionPtr)]SongFinishedCallback songFinishedCallback);

		abstract internal double AbsGetVolume();

		abstract internal void AbsSetVolume(double volume);
    }
}
