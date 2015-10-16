using System;

namespace Noised.Core.Plugins
{
	/// <summary>
	///		Basic plugin installer installing *.npluginz files
	/// </summary>
	public interface IPluginInstaller
	{
		/// <summary>
		///		Installs all npluginz files from a given path
		/// </summary>
		/// <param name="path">The path containing the npluginz files to install </param>
		void InstallAll(String path);

		/// <summary>
		///		Installs a noised plugin from a npluginz file
		/// </summary>
		void Install(String npluginzPath);
	};
}
