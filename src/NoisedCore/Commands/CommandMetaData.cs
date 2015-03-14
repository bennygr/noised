using System;
using System.Collections.Generic;
namespace Noised.Core.Commands
{
	public class CommandMetaData
	{
		public float ProtocolVersion{get;set;}
		public String Name{get;set;}
		public List<Object> Parameters{get;set;}
	}

}
