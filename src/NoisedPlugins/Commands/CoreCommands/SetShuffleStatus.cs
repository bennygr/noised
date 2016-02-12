using Noised.Core.Commands;
using Noised.Core.IOC;
using Noised.Core.Media;
using Noised.Core.Service;

namespace Noised.Plugins.Commands.CoreCommands
{
    public class SetShuffleStatus : AbstractCommand
    {
        private readonly bool shuffleStatus;

        public SetShuffleStatus(IServiceConnectionContext context, bool shuffleStatus):base(context)
        {
            this.shuffleStatus = shuffleStatus;
        }

        #region Overrides of AbstractCommand

        protected override void Execute()
        {
            IocContainer.Get<IMediaManager>().Shuffle = shuffleStatus;
        }

        #endregion
    }
}
