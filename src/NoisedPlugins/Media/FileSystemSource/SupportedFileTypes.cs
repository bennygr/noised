using System.Collections.Generic;
using System.IO;
using Noised.Core.Config;
using Noised.Core.IOC;

/// <summary>
///		Util helper class for checking if a file system item is a supported MediaItem
/// </summary>
static class SupportedFileTypes
{
	/// <summary>
	///		Check if the given file is a supported media item
	/// </summary>
	/// <param name="file">The file to check</param>
	/// <returns>True, if the given file is a supported media item, false otherwise</returns>
	internal static bool IsFileSupported(FileSystemInfo file)
	{
		string fileTypesValue = IocContainer.Get<IConfig>().GetProperty(FileSystemSourceProperties.FileTypes);
		if(!string.IsNullOrEmpty(fileTypesValue))
		{
			var fileTypes = new List<string>(fileTypesValue.Split(FileSystemSourceProperties.SplitCharacter));
			var fileType = fileTypes.Find(f => file.FullName.EndsWith(f));
			if(fileType != null)
			{
				return true;
			}
		}
		return false;
	}
};
