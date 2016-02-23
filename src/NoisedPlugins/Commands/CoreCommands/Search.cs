using System;
using System.Collections.Generic;
using System.Linq;
using Noised.Core.Commands;
using Noised.Core.IOC;
using Noised.Core.Media;
using Noised.Core.Service;

namespace Noised.Plugins.Commands.CoreCommands
{
    /// <summary>
    /// Command that executes a search for MediaItems
    /// </summary>
    public class Search : AbstractCommand
    {
        private readonly string searchPattern;
        private readonly List<object> sourceIdentifiers;

        /// <summary>
        /// Command that executes a search for MediaItems
        /// </summary>
        /// <param name="context">ConnectionContext</param>
        /// <param name="searchPattern">What to search for</param>
        public Search(ServiceConnectionContext context, string searchPattern)
            : this(context, searchPattern, null)
        { }

        /// <summary>
        /// Command that executes a search for MediaItems
        /// </summary>
        /// <param name="context">ConnectionContext</param>
        /// <param name="searchPattern">What to search for</param>
        /// <param name="sourceIdentifiers">Where to search it</param>
        public Search(ServiceConnectionContext context, string searchPattern, List<object> sourceIdentifiers)
            : base(context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (String.IsNullOrWhiteSpace(searchPattern))
                throw new ArgumentNullException("searchPattern");

            if (sourceIdentifiers == null)
                sourceIdentifiers = new List<object>();

            this.searchPattern = searchPattern;
            this.sourceIdentifiers = sourceIdentifiers;
        }

        #region implemented abstract members of AbstractCommand

        protected override void Execute()
        {
            Context.SendResponse(new ResponseMetaData
            {
                Name = "Noised.Commands.Core.Search",
                Parameters =
                    new List<Object>
                    {
                        IocContainer.Get<IMediaSourceAccumulator>()
                            .Search(searchPattern, sourceIdentifiers.OfType<string>())
                    }
            });
        }

        #endregion
    };
}
