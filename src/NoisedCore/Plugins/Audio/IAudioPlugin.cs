using System;
using System.Collections.Generic;
using Noised.Core.Media;

namespace Noised.Core.Plugins.Audio
{
	/// <summary>
	///		A plugin for audio output
	/// </summary>
	public interface IAudioPlugin : IPlugin
	{
		/// <summary>
		///		A list of supported protocols to play
		/// </summary>
		/// <remarks>
		///		For example file://, spotify://, etc
		/// </remarks>
		IEnumerable<string> SupportedProtocols{get;}

		/// <summary>
		///		Plays the specified item 
		/// </summary>
		/// <param name="item">The item to play</param>
		void Play(MediaItem item);

		/// <summary>
		///		Play the specified item from a certain position
		/// </summary>
		/// <param name="item">The item to play</param>
		/// <param name="pos">The position from which to play playback in milliseconds</param>
		void Play(MediaItem item, int pos);

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
