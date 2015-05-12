using System.Collections.Generic;
namespace Noised.Core.Config.File
{
	/// <summary>
	///		Reads configuratition files
	/// </summary>
	interface IConfigFileReader
	{
		List<string> Read();
	};
}
