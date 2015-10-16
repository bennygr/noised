namespace Noised.Plugins.SDK
{
	public class NoisedFile
	{
		/// <summary>
		///		The full path to the file whilch should be added to the plugin
		/// </summary>
		public string FileSource{get;set;}

		/// <summary>
		///		The name of the subdirectory to create for the file, or null if the plugin's root
		///		directory should be used
		/// </summary>
		public string DestinationSubDirectory{get;set;}
	};
}
