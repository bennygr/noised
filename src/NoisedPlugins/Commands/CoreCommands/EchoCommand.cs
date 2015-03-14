using System;
using Noised.Core.Commands;
using Noised.Core;

public class EchoCommand : AbstractCommand
{
	private string text; 
	public EchoCommand (string text)
		: base(null) 
	{ 
		this.text = text;
	}

	#region AbstractCommand
	
	protected override void Execute()
	{
		Console.WriteLine(text);
	}
	
	#endregion
};
