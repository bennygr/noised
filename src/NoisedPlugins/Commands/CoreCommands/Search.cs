using System;
using System.Collections.Generic;
using Noised.Core.Commands;
using Noised.Core.IOC;
using Noised.Core.Media;
using Noised.Core.Service;

namespace Noised.Plugins.Commands.CoreCommands
{
    /// <summary>
    ///		Command for Searching for MediaItems
    /// </summary>
    public class Search : AbstractCommand
    {
		private readonly string searchPattern;

        /// <summary>
        ///		Constructor
        /// </summary>
        /// <param name="context">Connection context</param>
        public Search(ServiceConnectionContext context, String searchPattern)
            : base(context)
        {
			this.searchPattern = searchPattern;
        }

        #region implemented abstract members of AbstractCommand

        protected override void Execute()
        {
            var sources = IocContainer.Get<IMediaSourceAccumulator>();
            var searchResults = sources.Search(searchPattern);
			Context.SendResponse(new ResponseMetaData{
						Name = "Noised.Commands.Core.Search",
						Parameters = new List<Object>{searchResults}
					});
        }

        #endregion
    };
}
