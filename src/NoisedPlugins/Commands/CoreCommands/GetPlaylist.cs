using System;
using System.Collections.Generic;
using Noised.Core.Commands;
using Noised.Core.IOC;
using Noised.Core.Media;
using Noised.Core.Service;

namespace Noised.Plugins.Commands.CoreCommands
{
    public class GetPlaylist : AbstractCommand
    {
        private readonly string playlistName;

        ///  <summary>
        /// 		Constructor
        ///  </summary>
        ///  <param name="context">The command's context</param>
        /// <param name="playlistName">Name of the Playlist</param>
        public GetPlaylist(ServiceConnectionContext context, string playlistName)
            : base(context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (playlistName == null)
                throw new ArgumentNullException("playlistName");

            this.playlistName = playlistName;
        }

        #region Overrides of AbstractCommand

        /// <summary>
        ///		Defines the command's behaviour
        /// </summary>
        protected override void Execute()
        {
            Playlist p = IocContainer.Get<IPlaylistManager>().FindPlaylist(playlistName);

            if (p == null)
                Context.SendResponse(new ErrorResponse("Could not find a Playlist named \"" + playlistName + "\""));

            Context.SendResponse(new ResponseMetaData
            {
                Name = "Noised.Plugins.Commands.CoreCommands.GetPlaylists",
                Parameters = new List<object> { p }
            });
        }

        #endregion
    }
}
