#---------------------------
PLUGIN_NAME=TcpService
#---------------------------
COMP=dmcs
OPTIONS = 
OUTPATH_RELEASE=../../../bin/release/plugins/
OUTPATH_DEBUG=../../../bin/debug/plugins/
CORE_OUT_RELEASE=../../../bin/release/NoisedCore.dll
CORE_OUT_DEBUG=../../../bin/debug/NoisedCore.dll
LOG_OUT_RELEASE=../../../bin/release/NoisedLogging.dll
LOG_OUT_DEBUG=../../../bin/debug/NoisedLogging.dll
#---------------------------
ifdef DEBUG
		OPTIONS += -debug
		OUTPATH=$(OUTPATH_DEBUG)
		CORE_OUT_PATH=$(CORE_OUT_DEBUG)
		LOG_OUT_PATH=$(LOG_OUT_DEBUG)
		DEBUGPATTERN=*.mdb
		DEBUGFILES=$(OUTPATH_DEBUG)$(DEBUGPATTERN)
else
		OUTPATH=$(OUTPATH_RELEASE)
		CORE_OUT_PATH=$(CORE_OUT_RELEASE)
		LOG_OUT_PATH=$(LOG_OUT_RELEASE)
endif
COMPILER=$(COMP) $(OPTIONS)
#---------------------------
all: Plugin 
#---------------------------
PLUGIN=$(shell find . -name "*.cs")
PLUGIN_OUT=$(OUTPATH)$(PLUGIN_NAME).dll
Plugin: $(PLUGIN_OUT)
$(PLUGIN_OUT): $(PLUGIN)
	$(COMPILER) $(PLUGIN) /t:library /r:$(CORE_OUT_PATH),$(LOG_OUT_PATH) /out:$(PLUGIN_OUT)
#---------------------------
