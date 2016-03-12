using System;

namespace Noised.Core.Media
{
    public interface IMetaFileAccumulator
    {
        void Refresh();

        void RefreshAsync();

        event Action RefreshAsyncFinished;
    }
}
