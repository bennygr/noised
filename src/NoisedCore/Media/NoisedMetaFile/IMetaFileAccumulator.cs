﻿using System;
using System.Threading.Tasks;

namespace Noised.Core.Media.NoisedMetaFile
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
        Task RefreshAsync();

        /// <summary>
        /// A calback that fires when the asynchronous refresh is finished
        /// </summary>
        event Action RefreshAsyncFinished;
    }
}
