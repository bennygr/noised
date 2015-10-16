using Noised.Core.DB;
using Noised.Core.Plugins;

namespace Noised.Core.DB
{
	/// <summary>
	///		Plugin for accessing the noised Database
	/// </summary>
	public interface IDB 
	{
		/// <summary>
		///		Updates or create the database if nececssary, 
		/// </summary>
		/// <returns>
		///		True, if the database has been updated, false if the update was not necessary 
		/// </returns>
		void CreateOrUpdate();
	};
}
