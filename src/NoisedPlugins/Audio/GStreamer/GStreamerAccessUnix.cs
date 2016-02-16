using System;
using System.Runtime.InteropServices;

namespace Noised.Plugins.Audio.GStreamer
{
	/// <summary>
	///		Provides Interop access to native gstreamer code
	/// </summary>
	class GStreamerAccessUnix : AbstractGStreamerAccess
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

		[DllImport ("libNoisedGstreamerAudio.so")]
		internal static extern void Initialize();

		[DllImport ("libNoisedGstreamerAudio.so")]
		internal static extern void Free();

		[DllImport ("libNoisedGstreamerAudio.so")]
		internal static extern void Play(String uri);

		[DllImport ("libNoisedGstreamerAudio.so")]
		internal static extern void Stop();

		[DllImport ("libNoisedGstreamerAudio.so")]
		internal static extern void Pause();

		[DllImport ("libNoisedGstreamerAudio.so")]
		internal static extern void Resume();

		[DllImport ("libNoisedGstreamerAudio.so")]
		[return: MarshalAsAttribute(UnmanagedType.Bool)]
		internal static extern bool IsPlaying();

		[DllImport ("libNoisedGstreamerAudio.so")]
		[return: MarshalAsAttribute(UnmanagedType.Bool)]
		internal static extern bool IsPaused();

		[DllImport ("libNoisedGstreamerAudio.so")]
		internal static extern void SetSongFinishedCallback(
				[MarshalAs(UnmanagedType.FunctionPtr)]SongFinishedCallback songFinishedCallback);

		[DllImport ("libNoisedGstreamerAudio.so")]
        internal static extern bool SetVolume(double volume);

		[DllImport ("libNoisedGstreamerAudio.so")]
        [return: MarshalAsAttribute(UnmanagedType.R8)]
        internal static extern double GetVolume();
	};
}
