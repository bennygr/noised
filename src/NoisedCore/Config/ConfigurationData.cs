namespace Noised.Core.Config
{
	
	/// <summary>
	///		Represents a bunch of raw configuration data
	/// </summary>
	/// <remarks>
	///		This class represents a raw bunch of data containing
	///		one or more configuration properties. For example
	///		a complete configuraton file loaded from disk,
	///		or the whole configuration data from a Database 
	/// </remarks>
	public class ConfigurationData
	{
		/// <summary>
		///		The name of the configuration data
		/// </summary>
		public string Name{get;set;}
	
		/// <summary>
		///		Configuration data as string 
		/// </summary>
		/// <remarks>For example the whole content of a configuration file</remarks>
		public string Data{get;set;}
	};
}
