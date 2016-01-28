using System;
using System.Collections.Generic;
using System.Linq;
using Noised.Core.Commands;
using Noised.Core.IOC;
using Noised.Core.Media;
using Noised.Core.Service;

namespace Noised.Plugins.Commands.CoreCommands
{
    public class RemoveFromPlaylist : AbstractCommand
    {
        private readonly string playlistName;
        private readonly IList<string> mediaItemUris;

        ///  <summary>
        /// 		Constructor
        ///  </summary>
        ///  <param name="context">The command's context</param>
        /// <param name="playlistName">Name of the Playlist from which the Items should be removed</param>
        /// <param name="mediaItemUris">List of Uris of MediaItems that should be removed</param>
        public RemoveFromPlaylist(ServiceConnectionContext context, string playlistName, IList<string> mediaItemUris)
            : base(context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (String.IsNullOrWhiteSpace(playlistName))
            {
                ArgumentException argumentException = new ArgumentException("please provide a valid Playlist name", "playlistName");
                Context.SendResponse(new ErrorResponse(argumentException)
                {
                    Name = "Noised.Plugins.Commands.CoreCommands.RemoveFromPlaylist"
                });

                throw argumentException;
            }

            if (mediaItemUris == null ||
                !mediaItemUris.Any())
            {
                ArgumentNullException argumentNullException = new ArgumentNullException("mediaItemUris", "please provide valid MediaItemUris");
                Context.SendResponse(new ErrorResponse(argumentNullException)
                {
                    Name = "Noised.Plugins.Commands.CoreCommands.RemoveFromPlaylist"
                });

                throw argumentNullException;
            }

            this.playlistName = playlistName;
            this.mediaItemUris = mediaItemUris;
        }

        #region Overrides of AbstractCommand

        /// <summary>
        ///		Defines the command's behaviour
        /// </summary>
        protected override void Execute()
        {
            foreach (string mediaItemUri in mediaItemUris)
            {
                IocContainer.Get<IPlaylistManager>()
                    .FindPlaylist(playlistName)
                    .Remove(IocContainer.Get<IMediaSourceAccumulator>().Get(new Uri(mediaItemUri)));
            }
        }

        #endregion
    }
}
