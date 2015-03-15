using System;
using Noised.Core.Commands;
using Noised.Core;

public class Echo : AbstractCommand
{
	private string text; 
	public Echo (string text) : base(null) 
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
