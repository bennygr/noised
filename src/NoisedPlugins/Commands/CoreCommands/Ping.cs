using System;
using Noised.Core.Commands;
using Noised.Core.Service;

namespace Noised.Plugins.Commands.CoreCommands
{
    /// <summary>
    ///		A simple ping command
    /// </summary>
    public class Ping : AbstractCommand
    {
        #region Constructor

        /// <summary>
        ///		Constructor
        /// </summary>
        /// <param name="context">Connection context</param>
        public Ping(ServiceConnectionContext context)
            : base(context) { }

        #endregion

        #region AbstractCommand

        protected override void Execute()
        {
            ResponseMetaData response =
                new ResponseMetaData()
                {
                    Name = "Noised.Commands.Core.Pong",
                };
            Console.WriteLine("PING!!!!");
            Context.SendResponse(response);
        }

        #endregion
    };
}
