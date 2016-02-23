using System.Collections.Generic;
using Noised.Core.Commands;
using Noised.Core.IOC;
using Noised.Core.Media;
using Noised.Core.Service;

namespace Noised.Plugins.Commands.CoreCommands
{
    public class GetMediaSources : AbstractCommand
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">The command's context</param>
        public GetMediaSources(IServiceConnectionContext context)
            : base(context)
        { }

        #region Overrides of AbstractCommand

        /// <summary>
        /// Defines the command's behaviour
        /// </summary>
        protected override void Execute()
        {
            Context.SendResponse(new ResponseMetaData
            {
                Name = "Noised.Commands.Core.GetMediaSources",
                Parameters = new List<object>(IocContainer.Get<IMediaSourceAccumulator>().Sources)
            });
        }

        #endregion
    }
}
