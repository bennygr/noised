using System;

namespace Noised.Core.Media
{
    /// <summary>
    /// Interace for access to all MetaFile sources
    /// </summary>
    public interface IMetaFileAccumulator
    {
        /// <summary>
        /// Refreshs all MetaFiles from alle MetaFile sources
        /// </summary>
        void Refresh();

        /// <summary>
        /// Refreshs all MetaFiles from alle MetaFile sources asynchronous
        /// </summary>
        void RefreshAsync();

        /// <summary>
        /// A calback that fires when the asynchronous refresh is finished
        /// </summary>
        event Action RefreshAsyncFinished;
    }
}
