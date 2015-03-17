using System;
using Noised.Core;
using Noised.Core.Commands;
using Noised.Core.Service;

namespace Noised.Commands.Core
{
	/// <summary>
	///		A simple ping command
	/// </summary>
	public class Ping : AbstractCommand
	{
		#region Constructor
	
		/// <summary>
		///		Constructor
		/// </summary>
		public Ping(ServiceConnectionContext context)
			: base(context) { }
	
		#endregion
	
		#region AbstractCommand
	
		protected override void Execute()
		{
			Console.WriteLine("PING!!!!");	
		}
	
		#endregion
	};
}
