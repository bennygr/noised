using System.Collections.Generic;
using Noised.Core.Commands;
using Noised.Core.IOC;
using Noised.Core.Media;
using Noised.Core.Service;

namespace Noised.Plugins.Commands.CoreCommands
{
  public  class GetShuffleStatus : AbstractCommand
    {
        public GetShuffleStatus(IServiceConnectionContext context):base(context)
        { }

        #region Overrides of AbstractCommand

        protected override void Execute()
        {
            Context.SendResponse(new ResponseMetaData
            {
                Name = "Noised.Plugins.Commands.CoreCommands.GetShuffleStatus",
                Parameters = new List<object> { IocContainer.Get<IMediaManager>().Shuffle }
            });
        }

        #endregion
    }
}
