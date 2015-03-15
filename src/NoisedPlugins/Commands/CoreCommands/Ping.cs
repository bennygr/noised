using System;
using Noised.Core.Commands;
using Noised.Core;

public class Ping : AbstractCommand
{
	#region Constructor
	
	public Ping()
		: base(null) { }
	
	#endregion

	#region AbstractCommand
	
	protected override void Execute()
	{
		Console.WriteLine("PING!!!!");	
	}
	
	#endregion
};
