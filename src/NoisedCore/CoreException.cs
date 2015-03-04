using System;

namespace Noised.Core
{
	/// <summary>
	///		Main exception class used by the core
	/// </summary>
	public class CoreException : Exception 
	{
		/// <summary>
		///		Constrcutor
		/// </summary>
		public CoreException() {}

		/// <summary>
		///		Constrcutor
		/// </summary>
		/// <param name="message">Error message</param>
		public CoreException(string message)
	   		: base(message)	{}

		/// <summary>
		///		Constrcutor
		/// </summary>
		/// <param name="message">Error message</param>
		/// <param name="innerException">Inner Exception</param>
		public CoreException(string message, Exception innerException) 
			: base(message,innerException) {}
	};
}
