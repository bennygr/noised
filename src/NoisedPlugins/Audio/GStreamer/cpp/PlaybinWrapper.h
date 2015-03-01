//------------------------------------------------------------------------------

#ifndef PLAYBIN_WRAPPER_H
#define PLAYBIN_WRAPPER_H

//------------------------------------------------------------------------------
#include <string>
#include <gst/gst.h>
//------------------------------------------------------------------------------

class PlaybinWrapper
{
	public:
		GMainLoop *loop;
		bool isPlaying;
		bool isPaused;
		std::string name;
		GstElement *pipeline;

		PlaybinWrapper(std::string name)
		{
			this->name = name;
			this->isPlaying = false;

			int x=0;
			gst_init(&x, 0);
		}

		void PlayUri(std::string uri);
		void Stop();
		void Pause();
		void Resume();
		bool Seek(long ms);
};

//------------------------------------------------------------------------------

#endif

//------------------------------------------------------------------------------
//EOF
