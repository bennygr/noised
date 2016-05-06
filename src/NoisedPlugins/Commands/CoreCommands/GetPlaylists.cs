using System;
using System.Collections.Generic;
using Noised.Core.Commands;
using Noised.Core.DB;
using Noised.Core.Service;

namespace Noised.Plugins.Commands.CoreCommands
{
    public class GetPlaylists : AbstractCommand
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="context">The command's context</param>
        public GetPlaylists(ServiceConnectionContext context)
            : base(context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
        }

        #region Overrides of AbstractCommand

        /// <summary>
        ///    Defines the command's behaviour
        /// </summary>
        protected override void Execute()
        {
            using (IUnitOfWork unitOfWork = Context.DIContainer.Get<IUnitOfWork>())
            {
                Context.SendResponse(new ResponseMetaData
                    {
                        Name = "Noised.Commands.Core.GetPlaylists",
                        Parameters = new List<object>(unitOfWork.PlaylistRepository.GetAll())
                    });
            }
        }

        #endregion
    }
}
