using Mono.Data.Sqlite;

namespace Noised.Core.DB.Sqlite
{
	/// <summary>
	///		Factory for creating Sqlite connections
	/// </summary>
	public interface ISqliteConnectionFactory
	{
		/// <summary>
		///		Creates a new, still closed connection
		/// </summary>
		SqliteConnection Create();
	};
}
