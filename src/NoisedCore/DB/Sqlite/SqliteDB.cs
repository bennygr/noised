using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Noised.Core.IOC;
using Noised.Core.Resources;
using Noised.Logging;

namespace Noised.Core.DB.Sqlite
{
    public class SqliteDB : IDB
    {
        /// <summary>
        /// Gets a value that determines if the system runs a linux OS
        /// </summary>
        private static bool IsLinux
        {
            get
            {
                int p = (int)Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
            }
        }

        private static List<string> GenerateCreateStatements()
        {
            return new List<string>
            {
                PluginsSql.CREATE_TABLE_STMT,
                PluginFilesSql.CREATE_TABLE_STMT,
                PlaylistsSql.CreateTableStmt,
                PlaylistsSql.CreateItemsTableStmt,
                MetaFilesSql.CreateTableStmt,
                UserSql.CreateTableStmt
            };
        }

        #region IDB implementation

        public void CreateOrUpdate()
        {
            if (!IsLinux)
            {
                // If this code is executed on a non UNIX System (Windows) the Windows sqlite3.dll is extracted into the apllication folder
                string directoryName = Path.GetDirectoryName(Assembly.GetEntryAssembly().CodeBase);
                if (directoryName != null)
                {
                    string exeDir = directoryName.Replace("file:\\", string.Empty);
                    ResourceExtractor.ExtractEmbeddedResource(exeDir, "Noised.Core.Resources", new List<string> { "sqlite3.dll" });
                }
                else
                {
                    ILogging log = IoC.Get<ILogging>();
                    log.Error("Unable to extract SQLite library: unable to locate assembly.");
                }
            }

            new SqliteDBCreator().CreateOrUpdate(new SqliteCoreConnectionFactory(), GenerateCreateStatements());
        }

        #endregion
    };
}
