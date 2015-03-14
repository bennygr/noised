using System;
using Noised.Core.Commands;
using Noised.Core;

public class PingCommand : AbstractCommand
{
	#region Constructor
	
	public PingCommand()
		: base(null) { }
	
	#endregion

	#region AbstractCommand
	
	protected override void Execute()
	{
		Console.WriteLine("PING!!!!");	
	}
	
	#endregion
};
