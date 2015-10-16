using System;
using System.IO;
using System.Reflection;

namespace Noised.Core.DB.Sqlite
{
	static class SqliteFileSource
	{
	    private const string NOISED_DB_FILE_NAME = "noised.db";
	
		internal static String GetDBFileName()
		{
			return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
				   Path.DirectorySeparatorChar + 
				   NOISED_DB_FILE_NAME;
		}
	};
}
