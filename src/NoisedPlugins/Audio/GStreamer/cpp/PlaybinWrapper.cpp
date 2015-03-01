#include "PlaybinWrapper.h"
#include <iostream>

gboolean bus_call(GstBus *bus, GstMessage *msg, void *user_data)
{
	PlaybinWrapper* wrapper = (PlaybinWrapper*)user_data;
	switch (GST_MESSAGE_TYPE(msg)) {
		case GST_MESSAGE_EOS: 
		{
			g_main_loop_quit(wrapper->loop); 
			break;
		}
		case GST_MESSAGE_ERROR: 
		{
			GError *err;
			gst_message_parse_error(msg, &err, NULL);
			g_error("%s", err->message);
			g_error_free(err);
			g_main_loop_quit(wrapper->loop);
			break;
		}
		case GST_MESSAGE_STATE_CHANGED:
		{
			if (GST_MESSAGE_SRC (msg) == GST_OBJECT (wrapper->pipeline))
			{
				GstState old_state, new_state, pending_state;
				gst_message_parse_state_changed (msg,
								&old_state, 
								&new_state, 
								&pending_state);

				g_print ("\nPipeline state changed from %s to %s:\n",
					gst_element_state_get_name (old_state),
					gst_element_state_get_name (new_state));
			
				if( wrapper->isPlaying == false && 
				   wrapper->isPaused == false)
				{
					g_main_loop_quit(wrapper->loop);
					break;
				}

			}
			break;
		}

		default:
			break;
	}
	return true;
}

void PlaybinWrapper::PlayUri(std::string uri)
{
	GstBus *bus;

	loop = g_main_loop_new(NULL, FALSE);
	pipeline = gst_element_factory_make("playbin", "test");

	g_object_set(G_OBJECT(pipeline), "uri", uri.c_str(), NULL);
	bus = gst_pipeline_get_bus(GST_PIPELINE(pipeline));
	gst_bus_add_watch(bus, bus_call, this);
	gst_object_unref(bus);

	isPlaying = true;
	isPaused =false;

	gst_element_set_state(GST_ELEMENT(pipeline), GST_STATE_PLAYING);

	g_main_loop_run(loop);
	gst_element_set_state(GST_ELEMENT(pipeline), GST_STATE_NULL);
	gst_object_unref(GST_OBJECT(pipeline));
}

void PlaybinWrapper::Stop()
{
	isPlaying = false;
	isPaused = false;
	gst_element_set_state(GST_ELEMENT(pipeline),
		              GST_STATE_NULL);
}

void PlaybinWrapper::Pause()
{
	isPlaying = false;
	isPaused = true;
	gst_element_set_state(GST_ELEMENT(pipeline),
	 		      GST_STATE_PAUSED);
}

void PlaybinWrapper::Resume()
{
	if(isPaused == true)
	{
		isPlaying = true;
		isPaused = false;
		gst_element_set_state(GST_ELEMENT(pipeline), 
				      GST_STATE_PLAYING);
	}
}

bool PlaybinWrapper::Seek(long ms)
{
	if(isPlaying || isPaused )
	{
		return gst_element_seek_simple(
				GST_ELEMENT(pipeline),
				GST_FORMAT_TIME,
				GST_SEEK_FLAG_FLUSH, 
				ms*GST_MSECOND);
	}
	return false;
}
