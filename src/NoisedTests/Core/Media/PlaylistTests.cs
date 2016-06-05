using System;
using Noised.Core.Media;
using NUnit.Framework;
using Should;

namespace NoisedTests.Core.Media
{
    [TestFixture]
    public class PlaylistTests
    {
        [Test]
        public void MediaItemShouldBeAddable()
        {
            var playlist = new Playlist("testlist");

            playlist.Items.Count.ShouldEqual(0, "The Items count of a new Playlist should be 0.");
            playlist.GetNext().ShouldEqual(null, "The GetNext Method should return null if no Items are available.");

            var listable = new Listable<MediaItem>(new MediaItem(new Uri("http://validuri.io"), string.Empty));
            playlist.Add(listable);

            playlist.Items.Count.ShouldEqual(1, "After adding one MediaItem the Count should be 1.");
            playlist.GetNext()
                .ShouldEqual(listable, "The GetNext Method should return the only MediaItem if available.");
        }

        [Test]
        public void MediaItemShouldBeRemovable()
        {
            var playlist = new Playlist("testlist");

            playlist.Items.Count.ShouldEqual(0, "The Items count of a newly created Playlist should be 0.");
            playlist.GetNext()
                .ShouldEqual(null, "The GetNext Method of a Playlist should return null if no Items are available.");

            var listable = new Listable<MediaItem>(new MediaItem(new Uri("http://validuri.io"), string.Empty));
            playlist.Add(listable);

            playlist.Items.Count.ShouldEqual(1, "After adding an Item the Count should be 1.");
            playlist.GetNext().ShouldEqual(listable, "NextItem should return the just added Item.");

            playlist.Remove(listable);

            playlist.Items.Count.ShouldEqual(0, "After removing the only item on the list count should return 0.");
            playlist.GetNext()
                .ShouldEqual(null, "The NextItem Property of a Playlist should return null if no Items are available.");
        }

        [Test]
        public void MultipleMediaItemsShouldBeAddable()
        {
            var playlist = new Playlist("testlist");

            playlist.Items.Count.ShouldEqual(0, "After creation a Playlist should contain 0 Items.");
            playlist.GetNext().ShouldEqual(null, "When no Items are available NextItem should return null.");

            var listable1 = new Listable<MediaItem>(new MediaItem(new Uri("http://validuri.io"), string.Empty));
            var listable2 = new Listable<MediaItem>(new MediaItem(new Uri("http://validuri.io"), string.Empty));
            playlist.Add(listable1);
            playlist.Add(listable2);

            playlist.GetNext()
                .ShouldEqual(listable1,
                    "After adding Items the GetNext() Method should return the Items in the order they were added.");
            playlist.GetNext()
                .ShouldEqual(listable2,
                    "After adding Items the GetNext() Method should return the Items in the order they were added.");
            playlist.GetNext().ShouldEqual(null, "After returning all Items the GetNext() Method should return null.");

            playlist.Reset();

            playlist.GetNext()
                .ShouldEqual(listable1,
                    "After resetting the PlayedItems on the Playlist the GetNext() Method should return the Items in the order they were added.");
            playlist.GetNext()
                .ShouldEqual(listable2,
                    "After resetting the PlayedItems on the Playlist the GetNext() Method should return the Items in the order they were added.");
            playlist.GetNext().ShouldEqual(null, "After returning all Items the GetNext() Method should return null.");
        }

        [Test]
        public void NewPlaylistShouldReturnGivenName()
        {
            new Playlist("playlistName").Name.ShouldEqual("playlistName", "Playlists name is not equal to given name");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException),
            UserMessage = "Creating a Playlist with an empty name should return in an ArgumentException")]
        public void NewPlaylistWithEmptyNameShouldThrowException()
        {
            new Playlist(string.Empty);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException),
            UserMessage = "Creating a Playlist with null as the name should return in an ArgumentException")]
        public void NewPlaylistWithNoNameShouldThrowException()
        {
            new Playlist(null);
        }
    }
}