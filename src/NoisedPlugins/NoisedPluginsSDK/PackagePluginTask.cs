using System.Collections.Generic;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;


namespace Noised.Plugins.SDK
{
	/// <summary>
	///		Creates a noised plugin package
	/// </summary>
    public class CreatePluginPackage : Task
    {
        [Required]
        public  string Name{ get; set; }

		/// <summary>
		///		The files which should be added to the noised plugin package
		/// </summary>
        public ITaskItem[] PluginRuntimeFiles{ get; set; }
		
		/// <summary>
		///		The configuration files which should be added to the noised plugin package
		/// </summary>
        public ITaskItem[] PluginConfigurationFiles{ get; set; }

        #region implemented abstract members of Task

        public override bool Execute()
        {
			string packageFile = "bin" + Path.DirectorySeparatorChar + Name + ".npluginz";
			var files = new List<string>();
			if(PluginRuntimeFiles != null)
			{
				foreach(var file  in PluginRuntimeFiles)
				{
					files.Add(file.ToString());
				}
			}
			var configs = new List<string>();
			if(PluginConfigurationFiles != null)
			{
				foreach(var config in PluginConfigurationFiles)
				{
					configs.Add(config.ToString());
				}
			}

			new PluginPackager().CreatePlugin(packageFile,files,configs);
            return true;
        }

        #endregion
    };
}
