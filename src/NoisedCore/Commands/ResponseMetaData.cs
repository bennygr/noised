using System;
using System.Collections.Generic;

namespace Noised.Core.Commands
{
	/// <summary>
	///		Meta data for outgoing responses
	/// </summary>
	public class ResponseMetaData
	{
		public ResponseMetaData()
		{
			ProtocolVersion = 1.0f;
		}

		/// <summary>
		///		The version of the protocol
		/// </summary>
		public float ProtocolVersion{get;private set;}

		/// <summary>
		///		The name of the response
		/// </summary>
		public String Name{get;set;}

		/// <summary>
		///		Parameters of the response
		/// </summary>
		public List<Object> Parameters{get;set;}
	};
}
