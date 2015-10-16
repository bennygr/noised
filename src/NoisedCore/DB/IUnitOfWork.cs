using System;
using Noised.Core.DB;

namespace Noised.Core.DB
{
	/// <summary>
	///		A unit of work for accessing the database
	/// </summary>
	public interface IUnitOfWork : IDisposable 
	{
		/// <summary>
		///		Repository for accessing MediaItems
		/// </summary>
		IMediaItemRepository MediaItemRepository{get;}

		/// <summary>
		///		Repository for accesing PluginRegistration
		/// </summary>
		IPluginRepository IPluginRepository{get;} 

		/// <summary>
		///		Saves all changes made to the repositories 
		/// </summary>
		void SaveChanges();
	};
}
