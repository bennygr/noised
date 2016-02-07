using System;
using System.Collections.Generic;
using Noised.Core.Commands;
using Noised.Core.IOC;
using Noised.Core.Media;
using Noised.Core.Service;

namespace Noised.Plugins.Commands.CoreCommands
{
    public class Enqueue : AbstractCommand
    {
        private readonly IList<object> mediaItemURIs;

		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="context">The command's context</param>
		/// <param name="mediaItemURIs">A list of item URIs to add to the queue</param>
        public Enqueue(ServiceConnectionContext context, IList<object> mediaItemURIs)
            : base(context)
        {
            this.mediaItemURIs = mediaItemURIs;
        }

        #region implemented abstract members of AbstractCommand

        protected override void Execute()
        {
			var sources = IocContainer.Get<IMediaSourceAccumulator>();
			var queue = IocContainer.Get<IQueue>();
			foreach(string uriObject in mediaItemURIs)
			{
				var uriString = uriObject as String;
				var mediaItem = sources.Get(new Uri(uriString));
				if(mediaItem != null)
				{
					queue.Enqueue(new Listable<MediaItem>(mediaItem));	
					Context.Logging.Info(String.Format("Enqueued {0}", uriString));
				}
			}
        }

        #endregion

    };
}
