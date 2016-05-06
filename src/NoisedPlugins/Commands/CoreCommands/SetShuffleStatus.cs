using System;
using Noised.Core.Commands;
using Noised.Core.Media;
using Noised.Core.Service;

namespace Noised.Plugins.Commands.CoreCommands
{
    public class SetShuffleStatus : AbstractCommand
    {
        private readonly bool shuffleStatus;

        public SetShuffleStatus(IServiceConnectionContext context, bool shuffleStatus)
            : base(context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            this.shuffleStatus = shuffleStatus;
        }

        #region Overrides of AbstractCommand

        protected override void Execute()
        {
            Context.DIContainer.Get<IMediaManager>().Shuffle = shuffleStatus;
        }

        #endregion
    }
}
