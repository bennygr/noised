using System;
using Noised.Core.Service;
using Noised.Core.Commands;

namespace Noised.Commands.Core
{
	public class Login : AbstractCommand
	{
		private string userName;
		private string password;
		
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
				//TODO send response
			}
			else
			{
				Console.WriteLine("Access denied for user  " + userName);
				//TODO send response
			}
		}
	
		#endregion
	};
}
