using Noised.Core.Commands;
using Noised.Core.Media;
using Noised.Core.Service;

namespace Noised.Plugins.Commands.CoreCommands
{
    public class Stop : AbstractCommand
    {
        /// <summary>
        ///	Constructor
        /// </summary>
        /// <param name="context">Connection context</param>
        public Stop(ServiceConnectionContext context)
            : base(context) { }

        #region AbstractCommand

        protected override void Execute()
        {
            Context.DIContainer.Get<IMediaManager>().Stop();
        }

        #endregion
    };
}
