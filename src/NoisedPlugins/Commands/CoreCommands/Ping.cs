using System;
using Noised.Core;
using Noised.Core.Commands;
using Noised.Core.Service;

public class Ping : AbstractCommand
{
	#region Constructor
	
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
