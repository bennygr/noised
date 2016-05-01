using Noised.Logging;
using Noised.Core.IOC;
using Noised.Core.Commands;
using Noised.Core.UserManagement;

namespace Noised.Core.Service
{
    /// <summary>
    ///	    The context of a connection to a noised service
    /// </summary>
    public interface IServiceConnectionContext
    {
        /// <summary>
        ///     The conneciton to a client
        /// </summary>
        IServiceConnection Connection { get; }

        /// <summary>
        ///     Whether the client has been authenticated or not
        /// </summary>
        bool IsAuthenticated { get; set; }

        /// <summary>
        ///     A reference to the noised Logging
        /// </summary>
        ILogging Logging { get; }

        /// <summary>
        ///     The user related to the connection
        /// </summary>
        User User { get; set; }

        /// <summary>
        ///     Access to the services using the DI Container
        /// </summary>
        IDIContainer DIContainer {get; }

        /// <summary>
        ///     Sends a response to the client
        /// </summary>
        /// <param name="response">The response to send to the client</param>
        void SendResponse(ResponseMetaData response);
    }
}
