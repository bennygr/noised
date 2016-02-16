using System.Runtime.InteropServices;

namespace Noised.Plugins.Audio.GStreamer
{
    internal class GStreamerAccessWindows : AbstractGStreamerAccess
    {
        #region AbstractGStreamerAccess members

        internal override void AbsInitialize()
        {
            Initialize();
        }

        internal override void AbsFree()
        {
            Free();
        }

        internal override void AbsPlay(string uri)
        {
            Play(uri);
        }

        internal override void AbsStop()
        {
            Stop();
        }

        internal override void AbsPause()
        {
            Pause();
        }

        internal override void AbsResume()
        {
            Resume();
        }

        internal override bool AbsIsPlaying()
        {
            return IsPlaying();
        }

        internal override bool AbsIsPaused()
        {
            return IsPaused();
        }

        internal override void AbsSetSongFinishedCallback(SongFinishedCallback songFinishedCallback)
        {
            SetSongFinishedCallback(songFinishedCallback);
        }

		internal override double AbsGetVolume()
		{
			return GetVolume();
		}

		internal override void AbsSetVolume(double volume)
		{
			SetVolume(volume);
		}

        #endregion

        [DllImport("GStreamerCpp.dll")]
        internal static extern void Initialize();

        [DllImport("GStreamerCpp.dll")]
        internal static extern void Free();

        [DllImport("GStreamerCpp.dll")]
        internal static extern void Play(string uri);

        [DllImport("GStreamerCpp.dll")]
        internal static extern void Stop();

        [DllImport("GStreamerCpp.dll")]
        internal static extern void Pause();

        [DllImport("GStreamerCpp.dll")]
        internal static extern void Resume();

        [DllImport("GStreamerCpp.dll")]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        internal static extern bool IsPlaying();

        [DllImport("GStreamerCpp.dll")]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        internal static extern bool IsPaused();

        [DllImport("GStreamerCpp.dll")]
        internal static extern void SetSongFinishedCallback(
                [MarshalAs(UnmanagedType.FunctionPtr)]SongFinishedCallback songFinishedCallback);

        [DllImport("GStreamerCpp.dll")]
        internal static extern bool SetVolume(double volume);

        [DllImport("GStreamerCpp.dll")]
        [return: MarshalAsAttribute(UnmanagedType.R8)]
        internal static extern double GetVolume();
    }
}
