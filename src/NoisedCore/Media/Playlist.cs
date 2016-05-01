using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Noised.Core.Media
{
    /// <summary>
    /// A Playlist containing MediaItems
    /// </summary>
    public class Playlist
    {
        private readonly object locker = new Object();
        private readonly List<Listable<MediaItem>> items = new List<Listable<MediaItem>>();
        private readonly List<Listable<MediaItem>> returned = new List<Listable<MediaItem>>();
        private readonly Random random = new Random();
        private int position = -1;

        /// <summary>
        ///     Unique ID of the playlist
        /// </summary>
        public Int64 Id{ get; set; }

        /// <summary>
        /// Gets the Name of the Playlist
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the Items of the Playlist
        /// </summary>
        public IReadOnlyCollection<Listable<MediaItem>> Items
        {
            get
            {
                return new ReadOnlyCollection<Listable<MediaItem>>(items);
            }
        }

        /// <summary>
        /// A Playlist containing MediaItems
        /// </summary>
        /// <param name="name">Name of the Playlist</param>
        public Playlist(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Please provide a valid (non empty) name for the playlist.", "name");
            Name = name;
        }

        /// <summary>
        ///     Internal method to return the item from the given position
        /// </summary>
        /// <param name="position">The position to get the item from</param>
        /// <returns>The item for the given position</returns>
        private Listable<MediaItem> GetAt(int position)
        {
            var item = items[position];
            returned.Add(item);
            return item;
        }
        
        /// <summary>
        ///     Internal method to return a list of items which has not been returned yet
        /// </summary>
        /// <returns>A list of items which have not been returned yet</returns>
        private List<Listable<MediaItem>> GetNotReturned()
        {
            return items.FindAll(l => !returned.Contains(l));
        }

        /// <summary>
        /// Adds  MediaItems to the Playlist
        /// </summary>
        /// <param name="items">The MediaItems to add</param>
        public void Add(params Listable<MediaItem>[] items)
        {
            lock (locker)
            {
                this.items.AddRange(items);
            }
        }

        /// <summary>
        /// Removes a MediaItem from the Playlist
        /// </summary>
        /// <param name="mediaItem">MediaItem to remove</param>
        public void Remove(Listable<MediaItem> mediaItem)
        {
            if (mediaItem == null)
                throw new ArgumentNullException("mediaItem");
            lock (locker)
            {
                items.Remove(items.Find(x => x.ListId == mediaItem.ListId));
            }
        }

        /// <summary>
        ///  Returns if the playlist has unreturned items 
        /// </summary>
        /// <returns>Whether the playlist has unreturned items or not</returns>
        public bool HasUnreturnedItems()
        {
            return GetNotReturned().Count > 0;
        }

        /// <summary>
        ///	Get the next item from the playlist
        /// </summary>
        /// <returns>The next item from the playlist</returns>
        public Listable<MediaItem> GetNext()
        {
            lock (locker)
            {
                return (position < items.Count-1 && items.Count > 0) ? GetAt(++position) : null;
            }
        }

        /// <summary>
        ///     Gets a random item from the playlist which has not been returned yet
        /// </summary>
        /// <returns>An item which has not been returned yet</returns>
        public Listable<MediaItem> GetNextRandom()
        {
            lock (locker)
            {
                var candiates = GetNotReturned();
                if (candiates.Count > 0)
                {
                    int index = random.Next(0, candiates.Count - 1);
                    var ret = candiates[index];
                    returned.Add(ret);
                    return ret;
                }
                return null;
            }
        }

        /// <summary>
        ///     Clears the list of already returned items
        /// </summary>
        public void Reset()
        {
            lock (locker)
            {
                returned.Clear();
                position = -1;
            }
        }
    }
}
