using Noised.Core.Commands;
using Noised.Core.IOC;
using Noised.Core.Media;
using Noised.Core.Service;

namespace Noised.Plugins.Commands.CoreCommands
{
    public class ClearQueue : AbstractCommand
    {
        /// <summary>
        ///		Constructor
        /// </summary>
        /// <param name="context">The command's context</param>
        public ClearQueue(ServiceConnectionContext context)
            : base(context)
        {
        }

        #region implemented abstract members of AbstractCommand

        protected override void Execute()
        {
            IocContainer.Get<IQueue>().Clear();
        }

        #endregion
    };
}
