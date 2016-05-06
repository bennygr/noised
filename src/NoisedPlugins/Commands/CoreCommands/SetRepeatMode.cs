using System;
using Noised.Core.Commands;
using Noised.Core.Media;
using Noised.Core.Service;

namespace Noised.Plugins.Commands.CoreCommands
{
    public class SetRepeatMode : AbstractCommand
    {
        private RepeatMode rmRepeatMode;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">The command's context</param>
        /// <param name="repeatMode"></param>
        public SetRepeatMode(IServiceConnectionContext context, string repeatMode)
            : base(context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (String.IsNullOrWhiteSpace(repeatMode))
                throw new ArgumentNullException("repeatMode");

            rmRepeatMode = (RepeatMode)Enum.Parse(typeof(RepeatMode), repeatMode, true);
        }

        #region Overrides of AbstractCommand

        /// <summary>
        /// Defines the command's behaviour
        /// </summary>
        protected override void Execute()
        {
            Context.DIContainer.Get<IMediaManager>().Repeat = rmRepeatMode;
        }

        #endregion
    }
}
