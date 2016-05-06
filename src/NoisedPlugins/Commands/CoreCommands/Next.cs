using Noised.Core.Commands;
using Noised.Core.Media;
using Noised.Core.Service;

namespace Noised.Plugins.Commands.CoreCommands
{
    public class Next : AbstractCommand
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="context">Connection context</param>
        public Next(ServiceConnectionContext context)
            : base(context)
        {
        }

        #region implemented abstract members of AbstractCommand

        protected override void Execute()
        {
            Context.DIContainer.Get<IMediaManager>().ProcessNext();
        }

        #endregion

    };
}
