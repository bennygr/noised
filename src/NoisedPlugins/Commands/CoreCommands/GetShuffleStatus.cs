using System;
using System.Collections.Generic;
using Noised.Core.Commands;
using Noised.Core.Media;
using Noised.Core.Service;

namespace Noised.Plugins.Commands.CoreCommands
{
    public class GetShuffleStatus : AbstractCommand
    {
        /// <summary>
        ///	Constructor
        /// </summary>
        /// <param name="context">The command's context</param>
        public GetShuffleStatus(IServiceConnectionContext context)
            : base(context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
        }

        #region Overrides of AbstractCommand

        protected override void Execute()
        {
            Context.SendResponse(new ResponseMetaData
                {
                    Name = "Noised.Plugins.Commands.CoreCommands.GetShuffleStatus",
                    Parameters = new List<object> { Context.DIContainer.Get<IMediaManager>().Shuffle }
                });
        }

        #endregion
    }
}
