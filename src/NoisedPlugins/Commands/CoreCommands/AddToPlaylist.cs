using System;
using System.Collections.Generic;
using System.Linq;
using Noised.Core;
using Noised.Core.Commands;
using Noised.Core.IOC;
using Noised.Core.Media;
using Noised.Core.Service;

namespace Noised.Plugins.Commands.CoreCommands
{
    public class AddToPlaylist : AbstractCommand
    {
        private readonly string playlistName;
        private readonly IEnumerable<string> mediaItemUris;

        ///  <summary>
        /// 		Constructor
        ///  </summary>
        ///  <param name="context">The command's context</param>
        /// <param name="playlistName">Name of the Playlist to which the Items should be added</param>
        /// <param name="mediaItemUris">List of Uris of MediaItems that should be added</param>
        public AddToPlaylist(ServiceConnectionContext context, string playlistName, IList<string> mediaItemUris)
            : base(context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (String.IsNullOrWhiteSpace(playlistName))
            {
                ArgumentException argumentException = new ArgumentException(strings.NoValidPlaylistName, "playlistName");

                Context.SendResponse(new ErrorResponse(argumentException)
                {
                    Name = "Noised.Plugins.Commands.CoreCommands.AddToPlaylist"
                });

                throw argumentException;
            }

            if (mediaItemUris == null ||
                !mediaItemUris.Any())
            {
                ArgumentNullException argumentNullException = new ArgumentNullException("mediaItemUris");

                Context.SendResponse(new ErrorResponse(argumentNullException)
                {
                    Name = "Noised.Plugins.Commands.CoreCommands.AddToPlaylist"
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
                IPlaylistManager playlistManager = IocContainer.Get<IPlaylistManager>();
                Playlist playlist = playlistManager.FindPlaylist(playlistName);
                playlist.Add(IocContainer.Get<IMediaSourceAccumulator>().Get(new Uri(mediaItemUri)));
                playlistManager.SavePlaylist(playlist);
            }
        }

        #endregion
    }
}
