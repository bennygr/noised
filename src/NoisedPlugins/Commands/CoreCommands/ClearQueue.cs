using Noised.Core.Commands;
using Noised.Core.Media;
using Noised.Core.Service;

namespace Noised.Plugins.Commands.CoreCommands
{
    /// <summary>
    ///		Command for clearing the queue
    /// </summary>
    public class ClearQueue : AbstractCommand
    {
        public ClearQueue(ServiceConnectionContext context)
            : base(context)
        {
        }

        #region implemented abstract members of AbstractCommand

        protected override void Execute()
        {
            Context.DIContainer.Get<IQueue>().Clear();
        }

        #endregion
    };
}
