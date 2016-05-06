using Noised.Core.Commands;
using Noised.Core.Media;
using Noised.Core.Service;

namespace Noised.Plugins.Commands.CoreCommands
{
    public class Pause : AbstractCommand
    {
        /// <summary>
        ///	Constructor
        /// </summary>
        /// <param name="context">Connection context</param>
        public Pause(ServiceConnectionContext context)
            : base(context)
        {
        }

        #region AbstractCommand

        protected override void Execute()
        {
            var mediaManager = Context.DIContainer.Get<IMediaManager>();
            mediaManager.Pause();
        }

        #endregion
    };
}
