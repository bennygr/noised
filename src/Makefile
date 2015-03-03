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
all: makeDirectories \
	 NoisedLogging \
	 NoisedCore \
	 NoisedServer
#---------------------------
#Create output directories
makeDirectories: 
	@if [ ! -d ./bin ]; \
	then \
		mkdir ./bin/;\
	fi
	@if [ ! -d ./bin/release ]; \
	then \
		mkdir ./bin/release;\
	fi
	@if [ ! -d ./bin/release/plugins ]; \
	then \
		mkdir ./bin/release/plugins;\
	fi
	@if [ ! -d ./bin/debug ]; \
	then \
		mkdir ./bin/debug;\
	fi
	@if [ ! -d ./bin/debug/plugins ]; \
	then \
		mkdir ./bin/debug/plugins;\
	fi

#---------------------------
#Install nuget dependencies

#---------------------------
#NoisedLogging
LOGGING=$(shell find ./NoisedLogging/ -name "*.cs")
LOGGING_OUT=$(OUTPATH)NoisedLogging.dll
NoisedLogging: $(LOGGING_OUT)

$(LOGGING_OUT): $(LOGGING)
	$(COMPILER) $(LOGGING) /t:library /out:$(LOGGING_OUT)

#---------------------------
#NoisedCore
CORE=$(shell find NoisedCore/ -name "*.cs")
CORE_OUT=$(OUTPATH)NoisedCore.dll
LIGHTCORE_DLLS=$(shell find ./NoisedCore/packages/LightCore.1.5.1/lib/net4.0/ -bane "*.dll")
LIGHTCORE='./NoisedCore/packages/LightCore.1.5.1/lib/net40/LightCore.dll'
NoisedCore: $(CORE_OUT) lightCoreOutput nugetCore
$(CORE_OUT): $(LOGGING) $(NETWORK) $(CORE)
	$(COMPILER) $(CORE) /t:library /r:$(LOGGING_OUT),${NETWORK_OUT},${LIGHTCORE} /out:$(CORE_OUT)
nugetCore:
	@if [ ! -d ./NoisedCore/packages ]; \
	then \
		echo "Installing nuget packages..." && nuget install -o ./NoisedCore/packages ./NoisedCore/packages.config;\
	fi
lightCoreOutput:
	@cp ./NoisedCore/packages/LightCore.1.5.1/lib/net40/*.dll $(OUTPATH)


#---------------------------
#NoisedServer
SERVER=$(shell find NoisedServer/ -name "*.cs")
SERVER_OUT=$(OUTPATH)NoisedServer.exe
NoisedServer: $(SERVER_OUT)
$(SERVER_OUT): $(SERVER) $(CORE)
	$(COMPILER) $(SERVER) /r:$(LOGGING_OUT),$(CORE_OUT), /out:$(SERVER_OUT)
