using System;
using Noised.Core;
using Noised.Core.Commands;
using Noised.Core.DB;
using Noised.Core.IOC;
using Noised.Core.Media;
using Noised.Core.Service;

namespace Noised.Plugins.Commands.CoreCommands
{
    public class DeletePlaylist : AbstractCommand
    {
        private readonly long playlistId;

        ///  <summary>
        ///     Constructor
        ///  </summary>
        ///  <param name="context">The command's context</param>
        /// <param name="playlistId">ID of the Playlist that should be deleted</param>
        public DeletePlaylist(IServiceConnectionContext context, long playlistId)
            : base(context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            this.playlistId = playlistId;
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
                    unitOfWork.PlaylistRepository.Delete(playlist);
                    unitOfWork.SaveChanges();
                }
                else
                {
                    Context.SendResponse(new ErrorResponse("Playlist " + playlistId + " not found")
                        {
                            Name = "Noised.Plugins.Commands.CoreCommands.DeletePlaylist"
                        });
                }
            }
        }

        #endregion
    }
}
