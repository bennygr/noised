namespace Noised.Core.Config
{
	/// <summary>
	///		An interface to a noised configuration object
	/// </summary>
	public interface IConfig
	{
		/// <summary>
		///		Gets a configuration property by name
		/// </summary>
		/// <param name="property">The name of the property</param>
		/// <param name="defaultValue">The default value to return if the property with the given name does not exist</param>
		string GetProperty(string property, string defaultValue=null);
	};
}
