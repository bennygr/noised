using System;
using Noised.Core.Commands;
using Noised.Core.IOC;
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
        public CreatePlaylist(ServiceConnectionContext context, string name)
            : base(context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException("please provide a valid Playlist name", "name");

            this.name = name;
        }

        #region Overrides of AbstractCommand

        /// <summary>
        ///		Defines the command's behaviour
        /// </summary>
        protected override void Execute()
        {
            IocContainer.Get<IPlaylistManager>().AddPlaylist(IocContainer.Get<IPlaylistManager>().CreatePlaylist(name));
        }

        #endregion
    }
}
