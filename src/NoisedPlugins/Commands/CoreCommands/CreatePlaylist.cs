using System;
using Noised.Core;
using Noised.Core.Commands;
using Noised.Core.DB;
using Noised.Core.Media;
using Noised.Core.Service;

namespace Noised.Plugins.Commands.CoreCommands
{
    public class CreatePlaylist : AbstractCommand
    {
        private readonly string name;

        ///  <summary>
        /// 		Constructor
        ///  </summary>
        ///  <param name="context">The command's context</param>
        /// <param name="name">Name of the playlist</param>
        public CreatePlaylist(IServiceConnectionContext context, string name)
            : base(context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (String.IsNullOrWhiteSpace(name))
            {
                var argumentException = new ArgumentException(strings.NoValidPlaylistName, "name");
                Context.SendResponse(new ErrorResponse(argumentException)
                    {
                        Name = "Noised.Commands.Core.CreatePlaylist"
                    });

                throw argumentException;
            }

            this.name = name;
        }

        #region Overrides of AbstractCommand

        /// <summary>
        ///	Defines the command's behaviour
        /// </summary>
        protected override void Execute()
        {
            using (IUnitOfWork unitOfWork = Context.DIContainer.Get<IUnitOfWork>())
            {
                unitOfWork.PlaylistRepository.Create(new Playlist(name));
                unitOfWork.SaveChanges();
            }
        }

        #endregion
    }
}
