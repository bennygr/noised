using System.Collections.Generic;
using System.IO;
using Noised.Core.Config;

namespace Noised.Plugins.Media.FileSystemSource
{
    /// <summary>
    /// Util helper class for checking if a file system item is a supported MediaItem
    /// </summary>
    class SupportedFileTypes
    {
        private readonly IConfig config;
    
        internal SupportedFileTypes(IConfig config)
        {
            this.config = config;
        }
    
        /// <summary>
        ///	    Check if the given file is a supported media item
        /// </summary>
        /// <param name="file">The file to check</param>
        /// <returns>True, if the given file is a supported media item, false otherwise</returns>
        internal bool IsFileSupported(FileSystemInfo file)
        {
            string fileTypesValue = config.GetProperty(FileSystemSourceProperties.FileTypes);
            if (!string.IsNullOrEmpty(fileTypesValue))
            {
                var fileTypes = new List<string>(fileTypesValue.Split(FileSystemSourceProperties.SplitCharacter));
                var fileType = fileTypes.Find(file.FullName.EndsWith);
                if (fileType != null)
                {
                    return true;
                }
            }
            return false;
        }
    };
}
