using Noised.Core.Commands;
using Noised.Logging;
using Noised.Core.UserManagement;

namespace Noised.Core.Service
{
    public interface IServiceConnectionContext
    {
        IServiceConnection Connection { get; }
        bool IsAuthenticated { get; set; }
        ILogging Logging { get; }
        User User { get; set; }

        void SendResponse(ResponseMetaData response);
    }
}