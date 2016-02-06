using System;
using System.Collections.Generic;
using Noised.Logging;
using Noised.Core.Commands;
using Noised.Core.IOC;
using Noised.Core.Plugins;
using Noised.Core.Plugins.Audio;
using Noised.Core.Service;

namespace Noised.Core.Media
{
    public class MediaManager : IMediaManager
    {
        private static readonly object locker = new object();
        private readonly IPluginLoader pluginLoader;
        private readonly IQueue queue;
        private readonly ILogging logger;
        private IAudioPlugin currentAudioOutput;
        private MediaItem currentMediaItem;

        /// <summary>
        ///		Constructor
        /// </summary>
        /// <param name="pluginLoader">Pluginloader</param>
        /// <param name="logger">The logger</param>
        /// <param name="queue">The queue</param>
        public MediaManager(ILogging logger, IPluginLoader pluginLoader,IQueue queue)
        {
            this.logger = logger;
            this.pluginLoader = pluginLoader;
            this.queue = queue;
            foreach (IAudioPlugin audio in pluginLoader.GetPlugins<IAudioPlugin>())
            {
                audio.SongFinished += OnSongFinished;
            }
        }

        private void OnSongFinished(Object sender, AudioEventArgs args)
        {
			Listable<MediaItem> nextItem;
			logger.Info("Finished playing " + args.MediaItem.Uri);
			nextItem = queue.Dequeue();

            lock (locker)
            {
                currentMediaItem = null;
            }

			if(nextItem != null)
			{
				Play(nextItem.Item);
			}
        }

        private IAudioPlugin GetAudioOutputForItem(MediaItem item)
        {
            foreach (IAudioPlugin audio in 
					pluginLoader.GetPlugins<IAudioPlugin>())
            {
                foreach (string protocol in audio.SupportedProtocols)
                {
                    if (item.Uri != null &&
                        item.Uri.ToString().StartsWith(protocol, StringComparison.Ordinal))
                    {
                        return audio;
                    }
                }
            }

            return null;
        }

        private void BroadcastMessage(ResponseMetaData message)
        {
            IocContainer.Get<IServiceConnectionManager>().SendBroadcast(message);
        }

        #region IMediaManager

        public MediaItem CurrentMediaItem
        {
            get
            {
                lock (locker)
                {
                    return currentMediaItem;
                }
            }
        }

        public bool Shuffle { get; set; }
		
        public bool Repeat { get; set; }
		
        public void Play(MediaItem item)
        {
            lock (locker)
            {
                //Getting an appropriated plugin for the 
                IAudioPlugin audio = GetAudioOutputForItem(item);
                if (audio == null)
                {
                    throw new CoreException(
                        "Could not find an audio plugin supporting playback for " +
                        item.Uri);
                }

                //Stopping if another plugin is already playing
                if (currentAudioOutput != null)
                {
                    logger.Debug(String.Format("Stopping current audio playback " +
                            "through plugin {0}",
                            currentAudioOutput.GetMetaData().Name));
                    currentAudioOutput.Stop();
                }

                //Setting current audio plugin and play the song
                logger.Debug(String.Format("Using audio plugin {0} to play item {1}",
                        audio.GetMetaData().Name,
                        item.Uri));
                currentAudioOutput = audio;
                currentMediaItem = item;	
            }
            currentAudioOutput.Play(item);
            var broadcastMessage = new ResponseMetaData
            {
                Name = "Noised.Commands.Core.Play",
                Parameters = new List<Object>
                {
                    item
                }
            };
            BroadcastMessage(broadcastMessage);
        }

        public void Stop()
        {
            ResponseMetaData broadcastMessage = null;	
            lock (locker)
            {
                if (currentAudioOutput != null)
                {
                    currentAudioOutput.Stop();
                    broadcastMessage = new ResponseMetaData
                    {
                        Name = "Noised.Commands.Core.Stop",
                        Parameters = new List<Object>
                        {
                            currentMediaItem
                        }
                    };
                }
				currentMediaItem = null;
                currentAudioOutput = null;
            }

            if (broadcastMessage != null)
            {
                BroadcastMessage(broadcastMessage);
            }
        }

        public void Pause()
        {
            ResponseMetaData broadcastMessage = null;	
            lock (locker)
            {
                if (currentAudioOutput != null)
                {
                    currentAudioOutput.Pause();
                    broadcastMessage = new ResponseMetaData
                    {
                        Name = "Noised.Commands.Core.Pause",
                        Parameters = new List<Object>
                        {
                            currentMediaItem
                        }
                    };
                }
            }
            if (broadcastMessage != null)
            {
                BroadcastMessage(broadcastMessage);
            }
        }

        public void Resume()
        {
            ResponseMetaData broadcastMessage = null;	
            lock (locker)
            {
                if (currentAudioOutput != null)
                {
                    currentAudioOutput.Resume();
                    broadcastMessage = new ResponseMetaData
                    {
                        Name = "Noised.Commands.Core.Play",
                        Parameters = new List<Object>
                        {
                            currentMediaItem
                        }
                    };
                }
            }
            if (broadcastMessage != null)
            {
                BroadcastMessage(broadcastMessage);
            }
        }

        #endregion
    };
}
