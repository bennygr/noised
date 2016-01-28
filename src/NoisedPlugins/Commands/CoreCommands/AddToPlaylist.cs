using System;
using System.Collections.Generic;
using System.Linq;
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
            this.playlistName = playlistName;
            this.mediaItemUris = mediaItemUris;
            if (context == null)
                throw new ArgumentNullException("context");
            if (String.IsNullOrWhiteSpace(playlistName))
                throw new ArgumentException("please provide a valid Playlist name", "playlistName");
            if (mediaItemUris == null || !mediaItemUris.Any())
                throw new ArgumentNullException("mediaItemUris");
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
                    .Add(IocContainer.Get<IMediaSourceAccumulator>().Get(new Uri(mediaItemUri)));
            }
        }

        #endregion
    }
}
