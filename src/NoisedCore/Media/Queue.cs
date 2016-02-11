using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Noised.Core.Commands;
using Noised.Core.Service;

namespace Noised.Core.Media
{
    /// <summary>
    ///		Default implementaion of IQueue
    /// </summary>
    public class Queue : IQueue
    {
		private readonly List<Listable<MediaItem>> queue = new List<Listable<MediaItem>>();
        private readonly IServiceConnectionManager connectionManager;

		public Queue(IServiceConnectionManager connectionManager)
		{
            this.connectionManager = connectionManager;
		}

		private void AssertItemNotInQueue(Listable<MediaItem> item)
		{
			if(queue.Find(i => i.ItemID == item.ItemID) != null)
			{
				throw new ArgumentException(String.Format("An item with an ID of {0} already exists in the queue.",item.ItemID));
			}
		}

		private List<Listable<MediaItem>> EnqueueInternal(Listable<MediaItem> item)
		{
			AssertItemNotInQueue(item);
			queue.Add(item);
			return queue;
		}

		private void OnQueueChanged(IEnumerable<Listable<MediaItem>> newContent)
		{
			connectionManager.SendBroadcast(new ResponseMetaData {
                Name = "Noised.Commands.Core.GetQueue",
                Parameters = new List<object>(newContent.GetMediaItems())
            });
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
			IEnumerable<Listable<MediaItem>> newContent;
			lock(queue)
			{
				newContent = EnqueueInternal(mediaItem);
			}
			OnQueueChanged(newContent);
        }

		public void Enqueue(IEnumerable<Listable<MediaItem>> mediaItems)
		{
			IEnumerable<Listable<MediaItem>> newContent = null;
			lock(queue)
			{
				foreach(Listable<MediaItem> mediaItem in mediaItems)
				{
					newContent = EnqueueInternal(mediaItem);
				}
			}
			OnQueueChanged(newContent);
		}

        public Listable<MediaItem> Dequeue()
        {
			IEnumerable<Listable<MediaItem>> newContent = null;
			Listable<MediaItem> item;
            lock(queue)
			{
				item = queue.Count > 0 ? queue[0] : null;
				if(item != null)
				{
					queue.RemoveAt(0);
					newContent = queue;
				}
			}

			if(newContent != null)
			{
				OnQueueChanged(newContent);
			}
			return item;
        }

		public ReadOnlyCollection<Listable<MediaItem>> GetContent()
		{
			lock(queue)
			{
				return queue.AsReadOnly();
			}
		}

		public void Remove(long listID)
		{
			IEnumerable<Listable<MediaItem>> newContent = null;
			lock(queue)
			{
				if(queue.RemoveAll(m => m.ItemID == listID) > 0)
				{
					newContent = queue;
				}
			}

			if(newContent != null)
			{
				OnQueueChanged(queue);
			}
		}

        public void Clear()
        {
            lock(queue)
			{
				queue.Clear();
			}
			OnQueueChanged(new List<Listable<MediaItem>>());
        }

        #endregion
    };
}
