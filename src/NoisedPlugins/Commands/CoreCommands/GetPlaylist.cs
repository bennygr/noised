using System;
using System.Collections.Generic;
using Noised.Core.Commands;
using Noised.Core.DB;
using Noised.Core.IOC;
using Noised.Core.Media;
using Noised.Core.Service;

namespace Noised.Plugins.Commands.CoreCommands
{
    public class GetPlaylist : AbstractCommand
    {
        private readonly long playlistId;

        ///  <summary>
        ///     Constructor
        ///  </summary>
        ///  <param name="context">The command's context</param>
        /// <param name="playlistId">Id of the Playlist</param>
        public GetPlaylist(ServiceConnectionContext context, long playlistId)
            : base(context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            this.playlistId = playlistId;
        }

        #region Overrides of AbstractCommand

        /// <summary>
        ///		Defines the command's behaviour
        /// </summary>
        protected override void Execute()
        {
            var playlist = Context.DIContainer.Get<IUnitOfWork>().PlaylistRepository.GetById(playlistId);
            if (playlist != null)
            {
                Context.SendResponse(new ResponseMetaData
                    {
                        Name = "Noised.Commands.Core.GetPlaylist",
                        Parameters = new List<object>{ playlist }
                    });
            }
            else
            {
                Context.SendResponse(new ErrorResponse("Could not find a Playlist with id \"" + playlistId + "\""));
            }
        }

        #endregion
    }
}
