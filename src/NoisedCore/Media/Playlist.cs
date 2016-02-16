using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Noised.Core.IOC;

namespace Noised.Core.Media
{
    /// <summary>
    /// A Playlist containing MediaItems
    /// </summary>
    public class Playlist
    {
        private readonly List<Listable<MediaItem>> items;
        private readonly List<Listable<MediaItem>> notYetPlayed;

        private Listable<MediaItem> currentMediaItem;

        private Listable<MediaItem> CurrentMediaItem
        {
            get
            {
                return currentMediaItem;
            }
            set
            {
                currentMediaItem = value;
                lock (this)
                    notYetPlayed.Remove(CurrentMediaItem);
            }
        }

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
        /// Gets the next Item of the Playlist
        /// </summary>
        public Listable<MediaItem> NextItem
        {
            get
            {
                lock (this)
                {
                    if (notYetPlayed.Count == 0)
                    {
                        ResetAlreadyPlayedItems();

                        switch (IocContainer.Get<IMediaManager>().Repeat)
                        {
                            // If RepeatMode is None null will be returned
                            case RepeatMode.None:
                                {
                                    CurrentMediaItem = null;
                                    return null;
                                }
                            // If RepeatMode is RepeatSong the CurrentMediaItem will be returned
                            case RepeatMode.RepeatSong:
                                return CurrentMediaItem;
                            // If RepeatMode is Playlist the NextItem will be determined as usual
                        }
                    }

                    // Get a random Item when Shuffle is true
                    if (IocContainer.Get<IMediaManager>().Shuffle)
                    {
                        CurrentMediaItem = notYetPlayed[new Random().Next(items.Count)];
                        return CurrentMediaItem;
                    }

                    CurrentMediaItem = notYetPlayed.FirstOrDefault();
                    return CurrentMediaItem;
                }
            }
        }

        /// <summary>
        /// A Playlist containing MediaItems
        /// </summary>
        /// <param name="name">Name of the Playlist</param>
        internal Playlist(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Please provide a valid (non empty) name for the playlist.", "name");

            Name = name;

            items = new List<Listable<MediaItem>>();
            notYetPlayed = new List<Listable<MediaItem>>();
        }

        /// <summary>
        /// Adds a MediaItem to the Playlist
        /// </summary>
        /// <param name="item">MediaItem to add</param>
        public void Add(Listable<MediaItem> item)
        {
            lock (this)
            {
                items.Add(item);
                notYetPlayed.Add(item);
            }
        }

        /// <summary>
        /// Removes a MediaItem from the Playlist
        /// </summary>
        /// <param name="mediaItem">MediaItem to remove</param>
        public void Remove(Listable<MediaItem> mediaItem)
        {
            lock (this)
            {
                items.Remove(items.Find(x => x.ListId == mediaItem.ListId));
                notYetPlayed.Remove(notYetPlayed.Find(x => x.ListId == mediaItem.ListId));
            }
        }

        /// <summary>
        /// Resets the already played MediaItems
        /// </summary>
        public void ResetAlreadyPlayedItems()
        {
            lock (this)
            {
                foreach (Listable<MediaItem> mediaItem in Items)
                {
                    if (!notYetPlayed.Contains(mediaItem))
                        notYetPlayed.Add(mediaItem);
                }
            }
        }
    }
}
