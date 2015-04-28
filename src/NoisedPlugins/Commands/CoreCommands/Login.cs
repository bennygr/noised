using System;
using Noised.Logging;
using System.Collections.Generic;
using Noised.Core.Service;
using Noised.Core.Commands;

namespace Noised.Commands.Core
{
	public class Login : AbstractCommand
	{
		private string userName;
		private string password;
		private ILogging logging;
		
		#region Constructor
		
		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="context">The connection context</param>
		/// <param name="userName">The user's name</param>
		/// <param name="password">The password</param>
		public Login (ServiceConnectionContext context,
					  string userName,
					  string password)
			: base(context,
				   requiresAuthentication: false)
		{
			this.userName = userName;
			this.password = password;
			this.logging = context.Logging;
		}
		
		#endregion

		#region AbstractCommand
	
		protected override void Execute()
		{
			//Just a test impl.
			//TODO: query against an auth. service injected into the plugin
			//-->PluginInitializer
			if(userName == "benny" && 
			   password == "test")
			{
				Context.IsAuthenticated = true;
				Console.WriteLine("Access granted for user  " + userName);
				ResponseMetaData response = 
					new ResponseMetaData()
					{
						Name = "Noised.Commands.Core.Welcome",
						Parameters = new List<object>()
						{
							"Welcome to the noised server \\m/",
						},
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
