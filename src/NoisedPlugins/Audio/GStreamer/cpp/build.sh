#!/bin/bash
g++ -Wall -shared -fPIC -o libNoisedGstreamerAudio.so `pkg-config gstreamer-1.0 --cflags` *.cpp `pkg-config gstreamer-1.0 --libs`
