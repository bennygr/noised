OUTPATH_RELEASE=../../../bin/Release/plugins/
OUTPATH_DEBUG=../../../bin/Debug/plugins/
CORE_OUT_RELEASE=../../../bin/Release/NoisedCore.dll
CORE_OUT_DEBUG=../../../bin/Debug/NoisedCore.dll
#---------------------------
ifdef DEBUG
		OUTPATH=$(OUTPATH_DEBUG)
else
		OUTPATH=$(OUTPATH_RELEASE)
endif
#---------------------------
all: cpp 
#---------------------------
#---------------------------
.PHONY: cpp
cpp:
	g++ -Wall -shared -fPIC -o libNoisedGstreamerAudio.so \
	`pkg-config gstreamer-1.0 --cflags` ./cpp/*.cpp \
	`pkg-config gstreamer-1.0 --libs`;
	echo "Moving libNoisedGstreamerAudio.so to $(OUTPATH)";
	mv libNoisedGstreamerAudio.so $(OUTPATH)libNoisedGstreamerAudio.so
