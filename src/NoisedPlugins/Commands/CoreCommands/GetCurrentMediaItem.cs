using System;
using System.Collections.Generic;
using Noised.Core.Commands;
using Noised.Core.IOC;
using Noised.Core.Media;
using Noised.Core.Service;

namespace Noised.Plugins.Commands.CoreCommands
{
    public class GetCurrentMediaItem : AbstractCommand
    {
        /// <summary>
        ///		Constructor
        /// </summary>
        /// <param name="context">The command's context</param>
        public GetCurrentMediaItem(ServiceConnectionContext context) : base(context) { }

        #region implemented abstract members of AbstractCommand

        protected override void Execute()
        {
            var mediaManager = IocContainer.Get<IMediaManager>();
			var currentMediaItem = mediaManager.CurrentMediaItem;
            var response =
                new ResponseMetaData
                {
                    Name = "Noised.Commands.Core.GetCurrentMediaItem",
					Parameters = new List<Object>{currentMediaItem}
                };
			Context.SendResponse(response);
        }

        #endregion
    };
}
