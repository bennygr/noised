/// <summary>
///		Installs plugins	
/// </summary>
interface IPluginInstaller
{
	/// <summary>
	///		Installs plugins
	/// </summary>
	/// <param name="plugins">Full pathes of the plugins to install</param>
	///
	void  InstallPlugins(params string[] plugins);
};
