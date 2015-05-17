OUTPATH_RELEASE=./bin/Release/
OUTPATH_DEBUG=./bin/Debug/
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
	mkdir -p $(OUTPATH)
	mv libNoisedGstreamerAudio.so $(OUTPATH)libNoisedGstreamerAudio.so
