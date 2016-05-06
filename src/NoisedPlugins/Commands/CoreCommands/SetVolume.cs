using System;
using Noised.Core.Commands;
using Noised.Core.Media;
using Noised.Core.Service;

namespace Noised.Plugins.Commands.CoreCommands
{
    public class SetVolume : AbstractCommand
    {
        private readonly int volume;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="context">The command's context</param>
        /// <param name="volume">Volume to set+</param>
        public SetVolume(IServiceConnectionContext context, object volume)
            : base(context)
        {
            int v;

            if (context == null)
                throw new ArgumentNullException("context");
            if (!Int32.TryParse(volume.ToString(), out v))
                throw new ArgumentException("volume has to be an int type");

            this.volume = v;
        }

        #region Overrides of AbstractCommand

        /// <summary>
        ///     Defines the command's behaviour
        /// </summary>
        protected override void Execute()
        {
            Context.DIContainer.Get<IMediaManager>().Volume = volume;
        }

        #endregion
    }
}
