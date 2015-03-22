#include <iostream>
#include "PlaybinWrapper.h"

PlaybinWrapper *playbinWrapper;

extern "C" void Initialize()
{
	playbinWrapper = new PlaybinWrapper("gstreamer noised audio plugin");
}

extern "C" void Free()
{
	delete playbinWrapper;
	playbinWrapper = NULL;
}

extern "C" void Play(const char* Uri, int position)
{
	playbinWrapper->PlayUri(std::string(Uri));
}

extern "C" bool IsPaused()
{
	return playbinWrapper->isPaused;
}

extern "C" bool IsPlaying()
{
	return playbinWrapper->isPlaying;
}

extern "C" void Stop()
{
	playbinWrapper->Stop();
}

extern "C" void Pause()
{
	playbinWrapper->Pause();
}

extern "C" void Resume()
{
	playbinWrapper->Resume();
}

extern "C" void SetSongFinishedCallback(SongFinishedCallback songFinishedCallback)
{
	playbinWrapper->songFinishedCallback = songFinishedCallback;
	playbinWrapper->callbackRegistered= true;
	std::cout << "CALLBACK REGISTERED" << std::endl;
}
