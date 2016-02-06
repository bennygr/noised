using Noised.Core.Commands;
using Noised.Core.IOC;
using Noised.Core.Media;

namespace Noised.Core.Commands
{
    class ProcessQueueCommand : AbstractCommand
    {
        internal ProcessQueueCommand()
            : base(null)
        {
        }

        #region implemented abstract members of AbstractCommand

        protected override void Execute()
        {
            var queue = IocContainer.Get<IQueue>();
            Listable<MediaItem> nextItem = queue.Dequeue();
            if (nextItem != null)
            {
                IocContainer.Get<IMediaManager>().Play(nextItem.Item);
            }
        }

        #endregion
	
    };
}
