using System;
using System.Collections.Generic;
using Noised.Core.Commands;
using Noised.Core.IOC;
using Noised.Core.Media;
using Noised.Core.Service;

namespace Noised.Plugins.Commands.CoreCommands
{
    public class DeletePlaylist : AbstractCommand
    {
        private readonly string playlistName;

        ///  <summary>
        /// 		Constructor
        ///  </summary>
        ///  <param name="context">The command's context</param>
        /// <param name="playlistName">Name of the Playlist that should be deleted</param>
        public DeletePlaylist(ServiceConnectionContext context, string playlistName)
            : base(context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (String.IsNullOrWhiteSpace(playlistName))
            {
                string message = "please provide a valid Playlist name";
                Context.SendResponse(new ResponseMetaData
                {
                    Name = "Noised.Plugins.Commands.CoreCommands.DeletePlaylist",
                    Parameters = new List<object> { message }
                });

                throw new ArgumentException(message, "playlistName");
            }

            this.playlistName = playlistName;
        }

        #region Overrides of AbstractCommand

        /// <summary>
        ///		Defines the command's behaviour
        /// </summary>
        protected override void Execute()
        {
            IPlaylistManager playlistManager = IocContainer.Get<IPlaylistManager>();

            Playlist playlist = playlistManager.FindPlaylist(playlistName);

            if (playlist == null)
            {
                Context.SendResponse(new ResponseMetaData
                {
                    Name = "Noised.Plugins.Commands.CoreCommands.DeletePlaylist",
                    Parameters = new List<object> { "Playlist " + playlistName + " not found" }
                });
                return;
            }

            playlistManager.DeletePlaylist(playlist);
        }

        #endregion
    }
}
