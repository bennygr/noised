using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Noised.Core.Media
{
    /// <summary>
    ///		Defines a queue which contains MediaItems with high priority for playback
    /// </summary>
    public interface IQueue
    {
        /// <summary>
        ///		The current size of the queue
        /// </summary>
        int Count { get; }

        /// <summary>
        ///		Adds new MediaItems to the queue
        /// </summary>
        void Enqueue(params Listable<MediaItem>[] mediaItems);

        /// <summary>
        ///		Adds a bunch of MediaItems to the queue
        /// </summary>
        void Enqueue(IEnumerable<Listable<MediaItem>> mediaItems);

        /// <summary>
        ///		Removes the next MediaItem from the Queue and returns it
        /// </summary>
        Listable<MediaItem> Dequeue();

        /// <summary>
        ///		Gets the content of the queue
        /// </summary>
        ReadOnlyCollection<Listable<MediaItem>> GetContent();

        /// <summary>
        ///		Removes an item from the queue
        /// </summary>
        /// <param name="listID">The list id of the item to remove</param>
        void Remove(long listID);

        /// <summary>
        ///		Clears the whole Queue
        /// </summary>
        void Clear();
    };
}
