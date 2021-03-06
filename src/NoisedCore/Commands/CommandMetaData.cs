using System;
using System.Collections.Generic;

namespace Noised.Core.Commands
{
	/// <summary>
	///		Meta data for incoming commands 
	/// </summary>
	public class CommandMetaData
	{
		/// <summary>
		///		The version of the protocol
		/// </summary>
		public float ProtocolVersion{get;set;}
		
		/// <summary>
		///		The name of the command
		/// </summary>
		public String Name{get;set;}

		/// <summary>
		///		Parameters of the command
		/// </summary>
		public List<Object> Parameters{get;set;}
	}

}
