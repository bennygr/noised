using Noised.Core.Commands;
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
			var mediaManager = IocContainer.Get<IMediaManager>();
            var queue = IocContainer.Get<IQueue>();
			//Checking queue for new music 
			//TODO: look at the current playlist
            Listable<MediaItem> nextItem = queue.Dequeue();
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
