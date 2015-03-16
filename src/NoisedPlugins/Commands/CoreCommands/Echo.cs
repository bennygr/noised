using System;
using Noised.Core;
using Noised.Core.Commands;
using Noised.Core.Service;

public class Echo : AbstractCommand
{
	private string text; 

	public Echo (ServiceConnectionContext context, string text) : base(context) 
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
