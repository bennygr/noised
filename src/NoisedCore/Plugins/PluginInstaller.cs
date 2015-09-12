using System;
using System.IO;
using System.Linq;
using Noised.Logging;
using Noised.Core.IOC;
namespace Noised.Core.Plugins
{
    /// <summary>
    ///		Default plugin installer
    /// </summary>
    public class PluginInstaller : IPluginInstaller
    {
        #region IPluginInstaller implementation

        public void InstallAll(string path)
        {
			var files = Directory.GetFiles(path).Where(
							file => file.EndsWith(".npluginz", StringComparison.Ordinal) );
            foreach (var file in files)
            {
				IocContainer.Get<ILogging>().Debug(String.Format("Found new plugin {0}...",file));
				System.IO.Compression.ZipFile.ExtractToDirectory(file,"./");
				File.Delete(file);
				IocContainer.Get<ILogging>().Debug(String.Format("Plugin {0} has been installed.",file));
            }
        }

        #endregion
    };
}
