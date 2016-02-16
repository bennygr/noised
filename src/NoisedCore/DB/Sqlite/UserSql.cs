namespace Noised.Core.DB.Sqlite
{
    internal static class UserSql
    {
        internal const string CreateTableStmt = "CREATE TABLE IF NOT EXISTS [Users] (" +
            "[Username] TEXT NOT NULL," +
            "[Password] TEXT NOT NULL," +
            "PRIMARY KEY (Username, Password));";

        internal static string SelectAllusers = "SELECT Username, Password FROM Users;";
    }
}
