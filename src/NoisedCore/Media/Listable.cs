using System;
namespace Noised.Core.Media
{
	static class ListableHelpers
	{
		internal readonly static Object Locker = new object();
		internal static long Counter;
	}

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
