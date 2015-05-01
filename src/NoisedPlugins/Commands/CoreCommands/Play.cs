using System;
using Noised.Core.Commands;
using Noised.Core.IOC;
using Noised.Core.Media;
using Noised.Core.Service;

namespace Noised.Plugins.Commands.CoreCommands
{
    public class Play : AbstractCommand
    {
        private string uri;

        /// <summary>
        ///		Constructor
        /// </summary>
        /// <param name="context">Connection context</param>
        /// <param name="uri">Uri of the media item to play</param>
        public Play(ServiceConnectionContext context,
                     String uri)
            : base(context)
        {
            this.uri = uri;
        }

        #region AbstractCommand

        protected override void Execute()
        {
            var sources = IocContainer.Get<IMediaSourceAccumulator>();
            MediaItem mediaItem = sources.GetItem(new Uri(uri));
            if (mediaItem != null)
            {
                var mediaManager = IocContainer.Get<IMediaManager>();
                mediaManager.Play(mediaItem);
            }
        }

        #endregion
    };
}
