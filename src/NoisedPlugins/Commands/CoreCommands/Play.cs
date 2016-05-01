using System;
using Noised.Core.Commands;
using Noised.Core.Media;
using Noised.Core.Service;

namespace Noised.Plugins.Commands.CoreCommands
{
    public class Play : AbstractCommand
    {
        private readonly Uri uri;

        /// <summary>
        ///	Constructor
        /// </summary>
        /// <param name="context">Connection context</param>
        /// <param name="uri">Uri of the media item to play</param>
        public Play(ServiceConnectionContext context, String uri)
            : base(context)
        {
            this.uri = new Uri(uri);
        }

        #region AbstractCommand

        protected override void Execute()
        {
            var sources = Context.DIContainer.Get<IMediaSourceAccumulator>();
            var mediaItem = sources.Get(uri);
            if (mediaItem != null)
            {
                var mediaManager = Context.DIContainer.Get<IMediaManager>();
                mediaManager.Play(mediaItem);
            }
            else
            {
                Context.SendResponse(
                    new ErrorResponse(
                        new MediaItemNotFoundException("Sorry. We have no info about a MediaItem at \"" + uri + "\""))
                    {
                        Name = "Noised.Plugins.Commands.CoreCommands.Play"
                    });
            }
        }

        #endregion
    };
}
