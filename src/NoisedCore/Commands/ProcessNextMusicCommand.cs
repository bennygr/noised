using Noised.Core.IOC;
using Noised.Core.Media;

namespace Noised.Core.Commands
{
    class ProcessNextMusicCommand : AbstractCommand
    {
        internal ProcessNextMusicCommand()
            : base(null)
        {
        }

        #region implemented abstract members of AbstractCommand

        protected override void Execute()
        {
            var mediaManager = IoC.Get<IMediaManager>();

            // checking for repeat
            if (mediaManager.Repeat == RepeatMode.RepeatSong)
                mediaManager.Play(mediaManager.CurrentMediaItem);

            //Checking queue for new music 
            var queue = IoC.Get<IQueue>();
            Listable<MediaItem> nextItem = queue.Dequeue();

            // Checking current playlist for new music
            if (nextItem == null)
            {
                var playlistManager = IoC.Get<IPlaylistManager>();
                var loadedPlaylist = playlistManager.LoadedPlaylist;
                if (loadedPlaylist != null)
                {
                    var processor = IoC.Get<IProcessPlaylistStrategy>();
                    nextItem = processor.GetNextItem(loadedPlaylist);
                }
            }

            if (nextItem != null)
            {
                mediaManager.Play(nextItem.Item);
            }
            else
            {
                mediaManager.Stop();
            }
        }

        #endregion
    };
}
