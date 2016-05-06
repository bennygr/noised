using System;
using System.Collections.Generic;
using System.Linq;
using Noised.Core.Commands;
using Noised.Core.DB;
using Noised.Core.Media;
using Noised.Core.Service;

namespace Noised.Plugins.Commands.CoreCommands
{
    public class RemoveFromPlaylist : AbstractCommand
    {
        private readonly long playlistId;
        private readonly IList<string> mediaItemUris;

        ///<summary>
        ///    Constructor
        ///</summary>
        ///<param name="context">The command's context</param>
        ///<param name="playlistId">ID of the Playlist from which the Items should be removed</param>
        ///<param name="mediaItemUris">List of Uris of MediaItems that should be removed</param>
        public RemoveFromPlaylist(IServiceConnectionContext context, long playlistId, IList<string> mediaItemUris)
            : base(context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (mediaItemUris == null ||
                !mediaItemUris.Any())
            {
                var argumentNullException = new ArgumentNullException("mediaItemUris", "please provide valid MediaItemUris");
                Context.SendResponse(new ErrorResponse(argumentNullException)
                    {
                        Name = "Noised.Plugins.Commands.CoreCommands.RemoveFromPlaylist"
                    });

                throw argumentNullException;
            }

            this.playlistId = playlistId;
            this.mediaItemUris = mediaItemUris;
        }

        #region Overrides of AbstractCommand

        /// <summary>
        ///     Defines the command's behaviour
        /// </summary>
        protected override void Execute()
        {
            using (IUnitOfWork unitOfWork = Context.DIContainer.Get<IUnitOfWork>())
            {
                var playlist = unitOfWork.PlaylistRepository.GetById(playlistId);
                if (playlist != null)
                {
                    bool changed = false;
                    foreach (string mediaItemUri in mediaItemUris)
                    {
                        playlist.Remove(new Listable<MediaItem>(Context.DIContainer.Get<IMediaSourceAccumulator>().Get(new Uri(mediaItemUri))));
                        changed = true;
                    }
                    if (changed)
                    {
                        unitOfWork.PlaylistRepository.Update(playlist);
                    }
                }
                else
                {
                    Context.SendResponse(new ErrorResponse("Could not find a Playlist with id \"" + playlistId + "\""));
                }
            }
        }

        #endregion
    }
}
