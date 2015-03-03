using System;
using System.Runtime.InteropServices;

namespace Noised.Plugins.Audio.GStreamer
{
	/// <summary>
	///		Provides Interop access to native gstreamer code
	/// </summary>
	internal static class GStreamerAccessUnix
	{
		[DllImport ("libNoisedGstreamerAudio.so")]
		internal static extern void Initialize();

		[DllImport ("libNoisedGstreamerAudio.so")]
		internal static extern void Free();

		[DllImport ("libNoisedGstreamerAudio.so")]
		internal static extern void Play(String Uri);

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
	};
}
