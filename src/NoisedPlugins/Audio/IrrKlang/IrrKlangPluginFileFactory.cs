using System;
using System.IO;
using System.Net;
using IrrKlang;

namespace Noised.Plugins.Audio.IrrKlang
{
    /// <summary>
    /// Factory for creating input streams for IrrKlang
    /// </summary>
    internal class IrrKlangPluginFileFactory : IFileFactory
    {
        #region Implementation of IFileFactory

        public Stream openFile(string source)
        {
            if (source.StartsWith("http://") ||
                source.StartsWith("https://"))
                return new WebClient().OpenRead(source);

            if (source.StartsWith("file:///"))
                return new FileStream(source.Replace("file:///", String.Empty), FileMode.Open, FileAccess.Read);

            return new FileStream(source, FileMode.Open, FileAccess.Read);
        }

        #endregion
    }
}
