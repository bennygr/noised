ifdef RELEASE
release:
	xbuild /nologo /p:DefineConstants="UNIX" /p:UNIX=TRUE /p:Configuration=Release
else 
debug:
	xbuild /nologo /p:DefineConstants="UNIX" /p:UNIX=TRUE /p:Configuration=Debug
endif

clean:
	xbuild /nologo /t:clean
