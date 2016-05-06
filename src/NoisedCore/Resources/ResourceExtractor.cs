using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Noised.Core.IOC;
using Noised.Logging;

namespace Noised.Core.Resources
{
    public static class ResourceExtractor
    {
        /// <summary>
        /// Extracts Embedded Resources to a certain location
        /// </summary>
        /// <param name="outputDir">Destination Path</param>
        /// <param name="resourceLocation">Location of Resources in the Assembly</param>
        /// <param name="files">List of Names of Files to extract</param>
        public static void ExtractEmbeddedResource(string outputDir, string resourceLocation, List<string> files)
        {
            ILogging log = IoC.Get<ILogging>();

            foreach (string file in files)
            {
                log.Debug("Extraction of \"" + file + "\" ...");

                using (Stream stream = Assembly.GetCallingAssembly().GetManifestResourceStream(resourceLocation + "." + file))
                {
                    string path = Path.Combine(outputDir, file);
                    log.Debug("... into " + path);

                    if (File.Exists(path))
                    {
                        log.Debug(path + " already exists. Skipping extraction.");
                        continue;
                    }

                    using (FileStream fileStream = new FileStream(path, FileMode.Create))
                    {
                        for (int i = 0; i < stream.Length; i++)
                            fileStream.WriteByte((byte)stream.ReadByte());
                        fileStream.Close();
                        log.Debug("Extraction completed.");
                    }
                }
            }
        }
    }
}
