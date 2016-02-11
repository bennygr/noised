using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Noised.Core.Media
{
    public class Playlist
    {
        private readonly List<MediaItem> items;

        private MediaItem currentMediaItem;

        public string Name { get; private set; }

        public IReadOnlyCollection<MediaItem> Items
        {
            get
            {
                return new ReadOnlyCollection<MediaItem>(items);
            }
        }

        internal Playlist(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Please provide a valid (non empty) name for the playlist.", "name");

            Name = name;

            items = new List<MediaItem>();
        }

        public void Add(MediaItem item)
        {
            lock (this)
                items.Add(item);
        }

        public MediaItem GetNextItem()
        {
            lock (this)
            {
                if (currentMediaItem == null)
                {
                    currentMediaItem = items.First();
                    return currentMediaItem;
                }

                return items.ElementAtOrDefault(items.IndexOf(currentMediaItem) + 1);
            }
        }

        public void Remove(MediaItem mediaItem)
        {
            lock (this)
                items.Remove(items.Find(x => x.Uri == mediaItem.Uri));
        }
    }
}
