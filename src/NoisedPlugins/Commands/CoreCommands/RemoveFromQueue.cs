using System.Collections.Generic;
using Noised.Core.Commands;
using Noised.Core.Media;
using Noised.Core.Service;

namespace  Noised.Plugins.Commands.CoreCommands
{
    public class RemoveFromQueue : AbstractCommand
    {
        private readonly IList<object> listIDs;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="context">The command's context</param>
        /// <param name="listIDs">A list of listIDs of the items to remove from the queue</param>
        public RemoveFromQueue(ServiceConnectionContext context, IList<object> listIDs)
            : base(context)
        {
            this.listIDs = listIDs;
        }

        #region implemented abstract members of AbstractCommand

        protected override void Execute()
        {
            var queue = Context.DIContainer.Get<IQueue>();
            foreach (var id in listIDs)
            {
                queue.Remove((long)id);
            }
        }

        #endregion
    };
}
