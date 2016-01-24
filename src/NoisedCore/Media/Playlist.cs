using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Noised.Core.Media
{
    public class Playlist
    {
        private readonly List<MediaItem> items;

        public string Name { get; private set; }

        public IReadOnlyCollection<MediaItem> Items
        {
            get
            {
                return new ReadOnlyCollection<MediaItem>(items);
            }
        }

        public Playlist(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Please provide a valid (non empty) name for the playlist.", "name");

            Name = name;

            items = new List<MediaItem>();
        }

        public void Add(MediaItem item)
        {
            items.Add(item);
        }
    }
}
