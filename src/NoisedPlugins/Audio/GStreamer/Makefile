#---------------------------
PLUGIN_NAME=GStreamerAudioPlugin
#---------------------------
COMP=dmcs
OPTIONS = 
OUTPATH_RELEASE=../../../bin/release/
OUTPATH_DEBUG=../../../bin/debug/
#TODO: put in a subfolder "plugins"
CORE_OUT_RELEASE=../../../bin/release/NoisedCore.dll
CORE_OUT_DEBUG=../../../bin/debug/NoisedCore.dll
#---------------------------
ifdef DEBUG
		OPTIONS += -debug
		OUTPATH=$(OUTPATH_DEBUG)
		CORE_OUT_PATH=$(CORE_OUT_DEBUG)
		DEBUGPATTERN=*.mdb
		DEBUGFILES=$(OUTPATH_DEBUG)$(DEBUGPATTERN)
else
		OUTPATH=$(OUTPATH_RELEASE)
		CORE_OUT_PATH=$(CORE_OUT_RELEASE)
endif
COMPILER=$(COMP) $(OPTIONS)
#---------------------------
all: cpp Plugin 
#---------------------------
PLUGIN=$(shell find . -name "*.cs")
PLUGIN_OUT=$(OUTPATH)$(PLUGIN_NAME).dll
Plugin: $(PLUGIN_OUT)
$(PLUGIN_OUT): $(PLUGIN)
	$(COMPILER) $(PLUGIN) /t:library /r:$(CORE_OUT_PATH) /out:$(PLUGIN_OUT)
#---------------------------
.PHONY: cpp
cpp:
	g++ -Wall -shared -fPIC -o libNoisedGstreamerAudio.so \
	`pkg-config gstreamer-1.0 --cflags` ./cpp/*.cpp \
	`pkg-config gstreamer-1.0 --libs`;
	mv libNoisedGstreamerAudio.so ../../../bin/release
