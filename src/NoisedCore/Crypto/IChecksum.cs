using System.IO;

namespace Noised.Core.Crypto
{
	/// <summary>
	///		Interface for calculating checksum
	/// </summary>
	public interface IChecksum
	{
		/// <summary>
		///		Calculates the checksum of a given file
		/// </summary>
		/// <returns>The checksum of the given file</returns>
		string CalculateChecksum(FileInfo file);
	};
}
