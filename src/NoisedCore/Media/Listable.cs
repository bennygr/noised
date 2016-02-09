using System;
using System.Collections.Generic;
namespace Noised.Core.Media
{
	/// <summary>
	///		Helper class containing internal helpers for dealing with Listable
	/// </summary>
	static class ListableHelpers
	{
		internal readonly static Object Locker = new object();
		internal static long Counter;
	}

	/// <summary>
	///		Extension methods for Listable
	/// </summary>
	public static class ListableExtensions
	{
		/// <summary>
		///		Helper method converting an enumeration of Listables to an enumeration of MediaItems
		/// </summary>
		public static IEnumerable<MediaItem> GetMediaItems(this IEnumerable<Listable<MediaItem>> listables)
		{
			foreach(var listable in listables)
			{
				yield return listable.Item;
			}
		}
	};

	/// <summary>
	///		Wrapper class for making an item appear in an ordered list while applying a unique ID to the item
	/// </summary>
	public class Listable<T>
	{
        private readonly T item;
        private readonly long itemID;

		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="item">The item to wrap and apply an ID for</param>
		public Listable(T item) 
		{
			lock(ListableHelpers.Locker)
			{
				this.itemID = ++ListableHelpers.Counter;
			}
			this.item = item;
		}

		/// <summary>
		///		Constructor
		/// </summary>
		/// <param name="item">The item to wrap and apply an ID for</param>
		/// <param name="itemID">The id to apply for the item</param>
		public Listable(T item, long itemID)
		{
            this.itemID = itemID;
            this.item = item;
		}

		/// <summary>
		///		The wrapped item
		/// </summary>
		public T Item
		{
			get
			{
				return item;
			}
		}

		/// <summary>
		///		The ID of the wrapped item
		/// </summary>
		public long ItemID
		{
			get
			{
				return itemID;
			}
		}
	};
}
