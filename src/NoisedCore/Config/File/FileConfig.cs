using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Noised.Logging;

namespace Noised.Core.Config.File
{
	/// <summary>
	///		Configuration which loads properties from the local filesystem
	/// </summary>
	public class FileConfig : IConfig
	{
#region Fields

		private string configPath;
		private readonly ILogging logging;
		private readonly Dictionary<string,string> properties;
		private const char CharComment = '#';
		private const char CharAssign = '=';

#endregion

#region Constructor

	
		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="logging">The logging object</param>
		public FileConfig(ILogging logging)
		{
			this.properties = new Dictionary<string,string>();
			this.logging = logging;
			this.configPath = 
				Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + 
				Path.DirectorySeparatorChar + 
				"etc";

			LoadAllConfigurationFile();
		}

#endregion

#region Methods

		private void LoadAllConfigurationFile()
		{
			logging.Debug(String.Format("Loading *.nconfig files from directory {0}", configPath));
			if (Directory.Exists(configPath))
			{
				//Loading all the nconfig files in the config folder
				var configFiles = Directory.GetFiles(configPath, "*.nconfig");
				foreach (var configFile in configFiles)
				{
					LoadConfigurationFile(configFile);
				}
			}
			else
			{
				logging.Error(String.Format("Could not load *.config files from directory {0}. Directory does not exist.", configPath));
			}
			logging.Debug(String.Format("{0} configuration properties loaded.", properties.Count));
		}

		private void LoadConfigurationFile(string fileName)
		{
			logging.Debug(String.Format("Loading configuration file {0}...", fileName));
			if(System.IO.File.Exists(fileName))
			{
				var streamReader = new StreamReader(fileName);
				string line;
				int lineCount = 0;
				//process each line
				while((line = streamReader.ReadLine()) != null)
				{
					lineCount++;
					if(!line.StartsWith(CharComment.ToString(), 
										StringComparison.Ordinal)) //Filter comments
					{
						var splits = line.Split(CharAssign);
						if(splits.Length == 2)
						{
                            var property = splits[0].Trim();
                            var value = splits[0].Trim();
							properties[property] = value;
							logging.Debug(String.Format("Added property {0}", property));
						}
						else
						{
							logging.Error(
								String.Format("Failed to load value {0}. Could not split line. At line #{1}", 
										      configPath,
											  lineCount));
						}
					}
				}
				logging.Debug(String.Format("Configuration file {0} successfully loaded.", fileName));
			}
			else
			{
				logging.Error(String.Format(
							  "Could not load configuration file {0}. File does not exist.", 
							  fileName));
			}
		}

#endregion


#region IConfig implementation

		public string GetProperty(string property, string defaultvalue=null)
		{
			return (properties.ContainsKey(property) ? properties[property] : defaultvalue);
		}

#endregion
	};

}
