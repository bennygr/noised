using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Noised.Core.Crypto
{
	public class Md5Checksum
    {

		/// <summary>
		///		Internal method create the hex string representation of the calcualted hash
		/// </summary>
		/// <returns>The hex sstring representation of the given hash</returns>
        private string HashToString(byte[] hash, bool upperCase = false)
        {
            var result = new StringBuilder(hash.Length * 2);
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString(upperCase ? "X2" : "x2"));
            }
            return result.ToString();			

        }

        #region IMediaItemChecksum implementation

        public string CalculateChecksum(FileInfo file)
        {
            using (var fileStream = File.OpenRead(file.FullName))
            using (var md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(fileStream);
                return HashToString(hash);
            }
        }

        #endregion
    };
}
