using System;
using System.Collections.Generic;
using Noised.Core.Commands;
using Noised.Core.IOC;
using Noised.Core.Media;
using Noised.Core.Service;

namespace Noised.Plugins.Commands.CoreCommands
{
    public class GetPlaylists : AbstractCommand
    {
        /// <summary>
        ///		Constructor
        /// </summary>
        /// <param name="context">The command's context</param>
        public GetPlaylists(ServiceConnectionContext context)
            : base(context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
        }

        #region Overrides of AbstractCommand

        /// <summary>
        ///		Defines the command's behaviour
        /// </summary>
        protected override void Execute()
        {
            Context.SendResponse(new ResponseMetaData
            {
                Name = "Noised.Commands.Core.GetPlaylists",
                Parameters = new List<object>(IocContainer.Get<IPlaylistManager>().Playlists)
            });
        }

        #endregion
    }
}
