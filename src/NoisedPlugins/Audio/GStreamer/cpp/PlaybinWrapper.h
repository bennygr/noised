//------------------------------------------------------------------------------

#ifndef PLAYBIN_WRAPPER_H
#define PLAYBIN_WRAPPER_H

//------------------------------------------------------------------------------
#include <string>
#include <gst/gst.h>
//------------------------------------------------------------------------------

typedef void (*SongFinishedCallback)(); 

class PlaybinWrapper
{
	public:
		GMainLoop *loop;
		bool isPlaying;
		bool isPaused;
		std::string name;
		GstElement *pipeline;
		SongFinishedCallback songFinishedCallback;
		bool callbackRegistered;

		PlaybinWrapper(std::string name)
		{
			this->name = name;
			this->isPlaying = false;
			this->isPaused = false;
			this->callbackRegistered = false;
			this->pipeline = NULL;
			int x=0;
			gst_init(&x, 0);
		}

		void PlayUri(std::string uri);
		void Stop();
		void Pause();
		void Resume();
		bool Seek(long ms);
		void SetVolume(double volume);
		double GetVolume();
};

//------------------------------------------------------------------------------

#endif

//------------------------------------------------------------------------------
//EOF
