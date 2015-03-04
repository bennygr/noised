using System;
using Noised.Core.Commands;
using Noised.Core;

public class PingCommand : AbstractCommand
{
	#region Constructor
	
	public PingCommand(User user)
		: base(user) { }
	
	#endregion

	#region AbstractCommand
	
	protected override void Execute()
	{
		Console.WriteLine("PING PING PING");	
	}
	
	#endregion
};
