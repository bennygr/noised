using System.Collections.Generic;
using Noised.Core.Commands;
using Noised.Core.Media;
using Noised.Core.Service;

namespace Noised.Plugins.Commands.CoreCommands
{
    /// <summary>
    ///	    Command for getting the Queues content
    /// </summary>
    public class GetQueue : AbstractCommand
    {
        /// <summary>
        ///	Constructor
        /// </summary>
        /// <param name="context">Connection context</param>
        public GetQueue(ServiceConnectionContext context)
            : base(context)
        {
        }

        #region implemented abstract members of AbstractCommand

        protected override void Execute()
        {
            var queue = Context.DIContainer.Get<IQueue>();
            Context.SendResponse(new ResponseMetaData
                {
                    Name = "Noised.Commands.Core.GetQueue",
                    Parameters = new List<object>(queue.GetContent())
                });
        }

        #endregion
    };
}
