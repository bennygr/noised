using System;
using System.Collections.Generic;

namespace Noised.Core.Commands
{
	public class ErrorResponse : ResponseMetaData
	{
		/// <summary>
		///		Exception related to this error
		/// </summary>
		public Exception Exception {get; private set;}

		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="e">Exception related to this error</param>
		public ErrorResponse (Exception e)
		{
			this.Exception = e;
			this.Name = "Noised.Core.Commmands.Error";
			this.Parameters = new List<Object>
							  {
								 e.Message,
							  };
		}
	};
}
