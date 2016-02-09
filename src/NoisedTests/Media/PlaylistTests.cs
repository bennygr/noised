//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Reflection;
//using Noised.Core.IOC;
//using Noised.Core.Media;
//using NUnit.Framework;
//using Should;
//using ResourceExtractor = NoisedTests.Resources.ResourceExtractor;

//namespace NoisedTests.Media
//{
//    [TestFixture]
//    public class PlaylistTests
//    {
//        private IPlaylistManager pman;

//        /// <summary>
//        /// Gets a value that determines if the system runs a linux OS
//        /// </summary>
//        private static bool IsLinux
//        {
//            get
//            {
//                int p = (int)Environment.OSVersion.Platform;
//                return (p == 4) || (p == 6) || (p == 128);
//            }
//        }

//        [SetUp]
//        public void PlaylistTestsSetup()
//        {
//            IocContainer.Build();

//            if (!IsLinux)
//            {
//                // If this code is executed on a non UNIX System (Windows) the Windows sqlite3.dll is extracted into the apllication folder
//                Assembly assembly = Assembly.GetAssembly(typeof(Noised.Core.ICore));
//                string directoryName = Path.GetDirectoryName(assembly.CodeBase);
//                if (directoryName != null)
//                {
//                    string exeDir = directoryName.Replace("file:\\", string.Empty);
//                    ResourceExtractor.ExtractEmbeddedResource(exeDir, "NoisedTests.Resources", new List<string> { "sqlite3.dll" });
//                }
//                else
//                    throw new Exception();
//            }
//            pman = IocContainer.Get<IPlaylistManager>();
//        }

//        [Test]
//        public void CreatePlaylistTest()
//        {
//            Playlist p = pman.CreatePlaylist("playlistName");
//            p.Name.ShouldEqual("playlistName");
//        }
//    }
//}
