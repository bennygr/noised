using System;
using System.Collections.Generic;
using System.Linq;
using Noised.Core.Commands;
using Noised.Core.DB;
using Noised.Core.IOC;
using Noised.Core.Media;
using Noised.Core.Service;

namespace Noised.Plugins.Commands.CoreCommands
{
    public class AddToPlaylist : AbstractCommand
    {
        private readonly long playlistId;
        private readonly IEnumerable<Object> mediaItemUris;

        ///  <summary>
        /// 		Constructor
        ///  </summary>
        ///  <param name="context">The command's context</param>
        /// <param name="playlistId">ID of the Playlist to which the Items should be added</param>
        /// <param name="mediaItemUris">List of Uris of MediaItems that should be added</param>
        public AddToPlaylist(IServiceConnectionContext context, long playlistId, List<Object> mediaItemUris)
            : base(context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (mediaItemUris == null ||
                !mediaItemUris.Any())
            {
                var argumentNullException = new ArgumentNullException("mediaItemUris");
                Context.SendResponse(new ErrorResponse(argumentNullException)
                    {
                        Name = "Noised.Commands.Core.AddToPlaylist",
                    });
                throw argumentNullException;
            }

            this.playlistId = playlistId;
            this.mediaItemUris = mediaItemUris;
        }

        #region Overrides of AbstractCommand

        /// <summary>
        ///		Defines the command's behaviour
        /// </summary>
        protected override void Execute()
        {
            var playlistRepository = Context.DIContainer.Get<IPlaylistRepository>();
            foreach (string mediaItemUri in mediaItemUris)
            {
                var playlist = playlistRepository.GetById(playlistId);
                if (playlist != null)
                {
                    playlist.Add(new Listable<MediaItem>(Context.DIContainer.Get<IMediaSourceAccumulator>().Get(new Uri(mediaItemUri))));
                }
                else
                {
                    Context.SendResponse(new ErrorResponse("Playlist not found")
                        {
                            Name = "Noised.Commands.Core.AddToPlaylist",
                        });
                }
            }
        }

        #endregion
    }
}
