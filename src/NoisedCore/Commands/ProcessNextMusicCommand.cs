using Noised.Core.IOC;
using Noised.Core.Media;

namespace Noised.Core.Commands
{
    class ProcessNextMusicCommand : AbstractCommand
    {
        internal ProcessNextMusicCommand()
            : base(null)
        { }

        #region implemented abstract members of AbstractCommand

        protected override void Execute()
        {
            var mediaManager = IocContainer.Get<IMediaManager>();
            var queue = IocContainer.Get<IQueue>();
            //Checking queue for new music 
            Listable<MediaItem> nextItem = queue.Dequeue();

            // If nothing was queued we check if a Playlist is loaded and if it can give us new music
            if (nextItem == null)
            {
                IPlaylistManager pman = IocContainer.Get<IPlaylistManager>();

                if (pman != null)
                {
                    Playlist p = pman.LoadedPlaylist;

                    if (p != null)
                        nextItem = p.NextItem;
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
