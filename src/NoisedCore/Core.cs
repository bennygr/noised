using System;
using System.Collections.Generic;
using System.Threading;
using Noised.Core.Commands;
using Noised.Core.IOC;
using Noised.Logging;

namespace Noised.Core
{
    /// <summary>
    ///		The noised core processing all incoming commands
    /// </summary>
    class Core : ICore
    {
        #region Fields

        private readonly Queue<AbstractCommand> commandQueue;
        private bool isRunning;
        private ILogging logging;
        private object locker = new object();

        #endregion

        #region Constructor

        /// <summary>
        ///		Constructor
        /// </summary>
        public Core()
        {
            commandQueue = new Queue<AbstractCommand>();
            logging = IocContainer.Get<ILogging>();
        }

        #endregion

        #region Methods

        /// <summary>
        ///		Internal method to process the core's command queue
        /// </summary>
        private void ProcessCommands()
        {
            isRunning = true;
            logging.Debug("noised core started");
            while (isRunning)
            {
                AbstractCommand cmd = null;
                lock (commandQueue)
                {
                    if (commandQueue.Count > 0)
                        cmd = commandQueue.Dequeue();
                }

                if (cmd != null)
                {
                    try
                    {
                        logging.Debug("Core is executing command " + cmd);
                        cmd.ExecuteCommand();
                    }
                    catch (Exception e)
                    {
                        logging.Error(String.Format("Error while executing command: {0}",
                                                    e.Message));
                        try
                        {
                            cmd.Context.SendResponse(new ErrorResponse(e));
                        }
                        catch
                        {
                            logging.Error(String.Format("Error while sending error to sender: {0}",
                                                        e.Message));
                        }
                    }
                    logging.Debug("Executed: " + cmd);
                }

                Thread.Sleep(1);
            }
            logging.Debug("noised core stopped");
        }

        #endregion

        #region ICore

        public bool IsRunning
        {
            get
            {
                lock (locker)
                {
                    return isRunning;
                }
            }
        }

        public void Start()
        {
            lock (locker)
            {
                if (isRunning)
                    throw new CoreException(
                        "Cannot start noised Core because it's still running");

                var coreThread = new Thread(ProcessCommands);
                coreThread.Start();
                logging.Debug("Starting noised core...");
            }
        }

        public void Stop()
        {
            lock (locker)
            {
                isRunning = false;
                logging.Debug("Stopping noised core...");
            }
        }

        public void AddCommand(AbstractCommand command)
        {
            lock (commandQueue)
            {
                commandQueue.Enqueue(command);
            }
        }

        #endregion
    };
}
