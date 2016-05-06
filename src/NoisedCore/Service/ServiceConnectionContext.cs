using System;
using Noised.Core.Commands;
using Noised.Core.IOC;
using Noised.Core.Service.Protocols;
using Noised.Logging;
using Noised.Core.UserManagement;

namespace Noised.Core.Service
{
    /// <summary>
    ///		A handle which wrapps a IServiceConnection at a higher level
    /// </summary>
    public class ServiceConnectionContext : IServiceConnectionContext
    {
        #region Fields

        private CommandDataBuffer commandBuffer;
        private readonly IProtocol protocol;
        private readonly ILogging logging;
        private ICore core;

        #endregion

        #region Constructor

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="core">The core</param>
        /// <param name="connection">The service connection to handle</param>
        /// <param name="protocol">noised protocol definition</param>
        internal ServiceConnectionContext(ICore core,
                                          IServiceConnection connection,
                                          IProtocol protocol)
        {
            connection.DataReceived += DataReceived;
            this.core = core;
            this.Connection = connection;
            this.commandBuffer = new CommandDataBuffer();
            this.protocol = protocol;
            this.DIContainer = IoC.Container;
            this.logging = IoC.Get<ILogging>();
        }

        #endregion
    
        #region Methods 

        /// <summary>
        ///	Method to handle received data from the connection
        /// </summary>
        private void DataReceived(object sender, ServiceEventArgs eventArgs)
        {
            if (commandBuffer.Add(eventArgs.Data))
            {
                string commandText;
                while ((commandText = commandBuffer.PopCommand()) != string.Empty)
                {
                    try
                    {
                        logging.Debug("Now creating command for:");
                        logging.Debug(commandText);
                        AbstractCommand command =
                            protocol.ParseCommand(this, commandText);
                        if (command != null)
                        {
                            //Checking authentication if required for the command
                            if (IsAuthenticated || !command.RequiresAuthentication)
                            {
                                //Synchronous command execution
                                core.ExecuteCommand(command);
                            }
                            else
                            {
                                string m = "Access denied for executing command: " + command;
                                //Access denied
                                logging.Error(m);
                                SendResponse(new ErrorResponse(m));
                            }
                        }
                        else
                        {
                            string m = "Error: Could not create command for text " +
                                       Environment.NewLine +
                                       commandText;
                            logging.Error(m);
                            SendResponse(new ErrorResponse(m));
                        }
                    }
                    catch (Exception e)
                    {
                        try
                        {
                            SendResponse(new ErrorResponse(e));
                            logging.Error(e.Message);
                        }
                        catch (Exception e2)
                        {
                            logging.Error("Error sending error response to client");
                            logging.Error(e2.Message);
                            logging.Error(e.StackTrace);
                        }
                    }
                }
            }
        }

        #endregion

        #region IServiceConnectionContext

        public IServiceConnection Connection { get; private set; }

        public bool IsAuthenticated { get; set; }

        public ILogging Logging { get { return logging; } }

        public User User { get; set; }

        public IDIContainer DIContainer {get; private set;}

        public void SendResponse(ResponseMetaData response)
        {
            logging.Debug("Sending response to client: " + response.Name);
            byte[] responseBytes = protocol.CreateResponse(response);
            Connection.Send(responseBytes);
        }
        #endregion

    };
}
