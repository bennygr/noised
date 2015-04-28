using System;
using System.Collections.Generic;

namespace Noised.Core.Commands
{
	public class ErrorResponse : ResponseMetaData
	{
		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="e">Exception related to this error</param>
		public ErrorResponse (Exception e) : this(e.Message) { }

		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="e">Error message related to this error</param>
		public ErrorResponse (string message)
		{
			this.Name = "Noised.Core.Commands.Error";
			this.Parameters = new List<Object>
							  {
								 message
							  };
		}
	};
}
