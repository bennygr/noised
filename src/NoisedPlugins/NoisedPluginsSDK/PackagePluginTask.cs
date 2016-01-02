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
        public string Name { get; set; }

        /// <summary>
        ///		The files which should be added to the noised plugin package
        /// </summary>
        public ITaskItem[] PluginRuntimeFiles { get; set; }

        /// <summary>
        ///		The configuration files which should be added to the noised plugin package
        /// </summary>
        public ITaskItem[] PluginConfigurationFiles { get; set; }

        #region implemented abstract members of Task

        public override bool Execute()
        {
            string metaDataFilePath = "plugin.nplugininfo";
            NoisedFile metaDataFile = new NoisedFile
            {
                FileSource = metaDataFilePath,
            };

            string packageFile = "bin" + Path.DirectorySeparatorChar + Name + ".npluginz";
            var files = new List<NoisedFile>();
            if (PluginRuntimeFiles != null)
            {
                foreach (var file in PluginRuntimeFiles)
                {
                    var subdir = file.GetMetadata("Subdir");
                    files.Add(new NoisedFile
                        {
                            FileSource = file.ToString(),
                            DestinationSubDirectory = subdir
                        });
                }
            }
            var configs = new List<NoisedFile>();
            if (PluginConfigurationFiles != null)
            {
                foreach (var config in PluginConfigurationFiles)
                {
                    var subdir = config.GetMetadata("Subdir");
                    configs.Add(new NoisedFile
                        {
                            FileSource = config.ToString(),
                            DestinationSubDirectory = subdir
                        });
                }
            }

            new PluginPackager().CreatePlugin(packageFile, metaDataFile, files, configs);
            return true;
        }

        #endregion
    };
}
