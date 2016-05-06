using System;
using System.Collections.Generic;
using Noised.Core.Commands;
using Noised.Core.Service;
using Noised.Core.UserManagement;
using Noised.Logging;

namespace Noised.Plugins.Commands.CoreCommands
{
    public class Login : AbstractCommand
    {
        private readonly string userName;
        private readonly string password;
        private readonly ILogging logging;

        #region Constructor

        /// <summary>
        ///	Constructor
        /// </summary>
        /// <param name="context">The connection context</param>
        /// <param name="userName">The user's name</param>
        /// <param name="password">The password</param>
        public Login(ServiceConnectionContext context, string userName, string password)
            : base(context, requiresAuthentication: false)
        {
            this.userName = userName;
            this.password = password;
            this.logging = context.Logging;
        }

        #endregion

        #region AbstractCommand

        protected override void Execute()
        {
            if (Context.DIContainer.Get<IUserManager>().Authenticate(userName, password))
            {
                Context.IsAuthenticated = true;
                Console.WriteLine("Access granted for user  " + userName);
                var response =
                    new ResponseMetaData
                    {
                        Name = "Noised.Commands.Core.Welcome",
                        Parameters = new List<object>
                        {
                            "Welcome to the noise \\m/",
                        }
                    };
                Context.SendResponse(response);
            }
            else
            {
                var errorResponse = new ErrorResponse("Invalid username or password");
                Context.SendResponse(errorResponse);
                logging.Warning("Invalid username or password. User: " + userName);
                Context.Connection.Close();
            }
        }

        #endregion
    };
}
