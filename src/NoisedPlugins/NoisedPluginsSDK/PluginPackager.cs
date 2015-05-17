using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;

/// <summary>
///		Packages a noised plugin 
/// </summary>
public class PluginPackager
{
    /// <summary>
    ///		Internal method to create a tmp directory
    /// </summary>
    /// <returns>The full path to the temp. directory</returns>
    private string CreateTmpDirectory()
    {
        string codeBase = Assembly.GetExecutingAssembly().CodeBase;
        var uri = new UriBuilder(codeBase);
        string assemblyName = Uri.UnescapeDataString(uri.Path);
        string tmpPath = Path.GetDirectoryName(assemblyName) +
                         Path.DirectorySeparatorChar +
                         "tmp" +
                         Path.DirectorySeparatorChar;
        //Clean tmp directory
        if (Directory.Exists(tmpPath))
        {
            Directory.Delete(tmpPath, true);
        }
        //Create a new 
        Directory.CreateDirectory(tmpPath);
        string contentPath = tmpPath + Path.DirectorySeparatorChar + "plugins";
        Directory.CreateDirectory(contentPath);
        string configPath = tmpPath + Path.DirectorySeparatorChar + "etc";
        Directory.CreateDirectory(configPath);
        return tmpPath;
    }

    /// <summary>
    ///		Internal method to remove the tmp directory
    /// </summary>
    /// <param name="path">The path to the tmp directory</param>
    private void DeleteTmpDirectory(string path)
    {
        Directory.Delete(path, true);
    }

    /// <summary>
    ///		Creates the plugin
    /// </summary>
	/// <param name="outputFile">
	///		The full path and file name where to save the created plugin package
	/// </param>
	/// <param name="pluginFiles">The plugin files to add to the plugin package</param>
	/// <param name="configurationFiles">The configuration files to add to the plugin package</param>
    public void CreatePlugin(string outputFile, 
							 IEnumerable<string> pluginFiles, 
							 IEnumerable<string> configurationFiles)
    {
        string tmpPath = CreateTmpDirectory();
        string contentPath = tmpPath + "plugins" + Path.DirectorySeparatorChar;
        string configPath = tmpPath + "etc" + Path.DirectorySeparatorChar;
        Console.WriteLine(tmpPath);

		//Copy plugin files
        foreach (var runtimeFile in pluginFiles)
        {
            var fileInfo = new FileInfo(runtimeFile);
            string fileDest = contentPath + fileInfo.Name;
            if (File.Exists(fileDest))
            {
                File.Delete(fileDest);
            }
            File.Copy(fileInfo.FullName, fileDest);
        }
		
		//Copy config files
        foreach (var configFile in configurationFiles)
        {
            var fileInfo = new FileInfo(configFile);
            string fileDest = configPath + fileInfo.Name;
            if (File.Exists(fileDest))
            {
                File.Delete(fileDest);
            }
            File.Copy(fileInfo.FullName, fileDest);
        }

		//delete old existing package
        if (File.Exists(outputFile))
        {
            File.Delete(outputFile);
        }
		//Create the package
        ZipFile.CreateFromDirectory(tmpPath, outputFile);

		//Cleanup
        DeleteTmpDirectory(tmpPath);
    }
};
