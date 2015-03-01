using System;
namespace Noised.Core.Plugins.Audio
{
	public interface IAudioPlugin : IPlugin
	{
		/// <summary>
		///		Plays the specified file 
		/// </summary>
		/// <param name="fileName">The name of the file to play</param>
		void Play(String fileName);

		/// <summary>
		///		Play the specified file from a certain position
		/// </summary>
		/// <param name="fileName">The name of the file to play</param>
		/// <param name="pos">The position from which to play playback in milliseconds</param>
		void Play(String fileName, int pos);

		/// <summary>
		///		Stops the playback
		/// </summary>
		void Stop();

		/// <summary>
		///		Pauses the playback
		/// </summary>
		void Pause();

		/// <summary>
		///		Resumes the playback
		/// </summary>
		void Resume();

		/// <summary>
		///		Whether the player is playing or not
		/// </summary>
		bool IsPlaying{get;}

		/// <summary>
		///		The current position of the playback in milliseconds
		/// </summary>
		int Position{get;set;}

		/// <summary>
		///		The length of the current playback in milliseconds
		/// </summary>
		int Length{get;}

		/// <summary>
		///		The current volume in percent
		/// </summary>
		int Volume{get;set;}
	};
}
