namespace Noised.Core.Config.File
{
	class FileConfigReader : IConfigFileReader
	{
		readonly string fileName;

		FileConfigReader (string fileName)
		{
			this.fileName = fileName;
		}

#region IConfigReader implementation 
		public string Read()
		{
			throw new System.NotImplementedException();
		}
#endregion
	};
}
