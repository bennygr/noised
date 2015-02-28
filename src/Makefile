#---------------------------
COMP=dmcs
OPTIONS = 
OUTPATH_RELEASE=bin/release/
OUTPATH_DEBUG=bin/debug/
#---------------------------
ifdef DEBUG
		OPTIONS += -debug
		OUTPATH=$(OUTPATH_DEBUG)
		DEBUGPATTERN=*.mdb
		DEBUGFILES=$(OUTPATH_DEBUG)$(DEBUGPATTERN)
else
		OUTPATH=$(OUTPATH_RELEASE)
endif
COMPILER=$(COMP) $(OPTIONS)

#---------------------------
#Everything
all: NoisedLogging NoisedNetwork

#---------------------------
#NoisedLogging
LOGGING=$(shell find ./NoisedLogging/ -name "*.cs")
LOGGING_OUT=$(OUTPATH)NoisedLogging.dll
NoisedLogging: $(LOGGING_OUT)

$(LOGGING_OUT): $(LOGGING)
	$(COMPILER) $(LOGGING) /t:library /out:$(LOGGING_OUT)

#---------------------------
#NoisedNetwork
NETWORK=$(shell find NoisedNetwork/ -name "*.cs")
NETWORK_OUT=$(OUTPATH)NoisedNetwork.dll
NoisedNetwork: $(NETWORK_OUT)
$(NETWORK_OUT): $(LOGGING) $(NETWORK)
	$(COMPILER) $(NETWORK) /t:library /r:$(LOGGING_OUT) /out:$(NETWORK_OUT)

