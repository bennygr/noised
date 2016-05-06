using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using Noised.Logging;
using Noised.Core.DB.Sqlite;
using Noised.Core.IOC;

namespace Noised.Core.DB.Sqlite
{
    /// <summary>
    ///		Class for creating SQLite Databases
    /// </summary>
    public class SqliteDBCreator
    {
        /// <summary>
        ///		Creates a database using the given set of statements
        /// </summary>
        /// <param name="connectionFactory">A factory used for connecting to the databse</param>
        /// <param name="statements">The set of statements to execute</param>
        public void CreateOrUpdate(ISqliteConnectionFactory connectionFactory, List<string> statements)
        {
            using (var connection = connectionFactory.Create())
            {
                connection.Open();
                foreach (var sql in statements)
                {
                    try
                    {
                        var command = new SqliteCommand(sql, connection);
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        var logger = IoC.Get<ILogging>();
                        logger.Error("Error while executing SQL: " +
                            sql +
                            " --> Error: " +
                            e.Message);
                        throw;
                    }
                }	
            }
        }
    };
}
