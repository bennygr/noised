namespace Noised.Core.Crypto
{
    /// <summary>
    /// Exposes basic Password functionalities
    /// </summary>
    public interface IPasswordManager
    {
        /// <summary>
        /// Verifies a given passwort against a hash
        /// </summary>
        /// <param name="password">The password that should be tested</param>
        /// <param name="hash">The hash the password is tested against</param>
        /// <returns>Returns true, when the password matches the hash. false otherwise.</returns>
        bool VerifyPassword(string password, string hash);

        /// <summary>
        /// Creates a hash for a password
        /// </summary>
        /// <param name="password">The password for which a hash should be created</param>
        /// <returns>Returns a hash for a password</returns>
        string CreateHash(string password);
    }
}
