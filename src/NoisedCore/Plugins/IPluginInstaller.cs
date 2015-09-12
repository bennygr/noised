using System;

namespace Noised.Core.Plugins
{
	/// <summary>
	///		Basic plugin installer installing *.npluginz files
	/// </summary>
	public interface IPluginInstaller
	{
		/// <summary>
		///		Installs all plugins from a given path
		/// </summary>
		/// <param name=""></param>
		void InstallAll(String path);
	};
}
