namespace Noised.Core.DB.Sqlite
{
    internal static class UserSql
    {
        internal const string CreateTableStmt = "CREATE TABLE IF NOT EXISTS [Users] (" +
            "[Username] TEXT NOT NULL," +
            "[Password] TEXT NOT NULL," +
            "PRIMARY KEY (Username));";
        internal const string GetUser = "SELECT Username, Password FROM Users where Username = @Username;";
        internal const string CreateUser = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password);";
        internal const string DeleteUser = "DELETE FROM Users where Username = @Username;";
    }
}
