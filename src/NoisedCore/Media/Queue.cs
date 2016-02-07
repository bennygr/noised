using System;
using System.Collections.Generic;

namespace Noised.Core.Media
{
    /// <summary>
    ///		Default implementaion of IQueue
    /// </summary>
    public class Queue : IQueue
    {
		private readonly List<Listable<MediaItem>> queue = new List<Listable<MediaItem>>();

		private void AssertItemNotInQueue(Listable<MediaItem> item)
		{
			if(queue.Find(i => i.ItemID == item.ItemID) != null)
			{
				throw new ArgumentException(String.Format("An item with an ID of {0} already exists in the queue.",item.ItemID));
			}
		}

		private void EnqueueInternal(Listable<MediaItem> item)
		{
			AssertItemNotInQueue(item);
			queue.Add(item);
		}

        #region IQueue implementation

        public int Count
        {
            get
            {
                lock(queue)
				{
					return queue.Count;
				}
            }
        }

        public void Enqueue(Listable<MediaItem> mediaItem)
        {
			lock(queue)
			{
				EnqueueInternal(mediaItem);
			}
        }

		public void Enqueue(IEnumerable<Listable<MediaItem>> mediaItems)
		{
			lock(queue)
			{
				foreach(Listable<MediaItem> mediaItem in mediaItems)
				{
					EnqueueInternal(mediaItem);
				}
			}
		}

        public Listable<MediaItem> Dequeue()
        {
            lock(queue)
			{
				var item = queue.Count > 0 ? queue[0] : null;
				if(item != null)
				{
					queue.RemoveAt(0);
				}
				return item;
			}
        }

		public void Remove(long listID)
		{
			lock(queue)
			{
				queue.RemoveAll(m => m.ItemID == listID);
			}
		}

        public void Clear()
        {
            lock(queue)
			{
				queue.Clear();
			}
        }

        #endregion
    };
}
