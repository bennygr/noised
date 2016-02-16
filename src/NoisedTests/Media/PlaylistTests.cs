using System;
using System.Collections.Generic;
using Moq;
using Noised.Core.DB;
using Noised.Core.IOC;
using Noised.Core.Media;
using NUnit.Framework;
using Should;

namespace NoisedTests.Media
{
    [TestFixture]
    public class PlaylistTests
    {
        private IPlaylistManager pman;

        [TestFixtureSetUp]
        public void OneTimeSetup()
        {
            IocContainer.Build();
        }

        [SetUp]
        public void BeforeEachMethodSetupCreateNewPlaylistManager()
        {
            Mock<IPlaylistRepository> mockPlaylistRepository = new Mock<IPlaylistRepository>();
            mockPlaylistRepository.Setup(x => x.AllPlaylists).Returns(new List<Playlist>());

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(x => x.PlaylistRepository).Returns(mockPlaylistRepository.Object);

            Mock<IDbFactory> mockFactory = new Mock<IDbFactory>();
            mockFactory.Setup(x => x.GetUnitOfWork()).Returns(mockUnitOfWork.Object);

            pman = new PlaylistManager(mockFactory.Object);
            pman.RefreshPlaylists();

            IocContainer.Get<IMediaManager>().Repeat = RepeatMode.None;
        }

        [Test]
        public void PlaylistManagerCreatePlaylist()
        {
            pman.CreatePlaylist("playlistName").ShouldNotBeNull("Playlist is null");
        }

        [Test]
        public void PlaylistManagerCreatePlaylistShouldReturnPlaylistWithGivenName()
        {
            pman.CreatePlaylist("playlistName").Name.ShouldEqual("playlistName", "Playlists name is not equal to given name");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), UserMessage = "Creating a Playlist with null as the name should return in an ArgumentException")]
        public void PlaylistManagerCreatePlaylistWithNullArgumentException()
        {
            pman.CreatePlaylist(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), UserMessage = "Creating a Playlist with aa empty name should return in an ArgumentException")]
        public void PlaylistManagerCreatePlaylistWithEmptyStringArgumentException()
        {
            pman.CreatePlaylist(String.Empty);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException), UserMessage = "Adding null instead of a Playlist should return in an ArgumentNullException")]
        public void PlaylistManagerAddPlaylistNullArgumentNullException()
        {
            pman.AddPlaylist(null);
        }

        [Test]
        [ExpectedException(typeof(PlaylistAlreadyExistsException), UserMessage = "Adding multiple Playslists with the same name should return in a PlaylisAlreadyExistsException")]
        public void PlaylistManagerAddPlaylistsWithSameNamePlaylistAlreadyExistsException()
        {
            pman.AddPlaylist(pman.CreatePlaylist("testlist"));
            pman.AddPlaylist(pman.CreatePlaylist("testlist"));
        }

        [Test]
        public void PlaylistManagerAddPlaylist()
        {
            pman.Playlists.Count.ShouldEqual(0, "Testsetup is invalid. This Method enters with at least one already existing Playlist.");

            Playlist playlist = pman.CreatePlaylist("testlist");
            pman.AddPlaylist(playlist);

            pman.Playlists.Count.ShouldEqual(1, "Adding of Playlist to Playlistmanager was not successful");

            pman.Playlists[0].Name.ShouldEqual(playlist.Name, "The existing Playlist does not have the same name as the added Playlist.");
            pman.Playlists[0].Items.ShouldEqual(playlist.Items, "The existing Playlist does not have the same Items as the added Playlist.");
        }

        [Test]
        public void PlaylistManagerFindPlaylist()
        {
            pman.FindPlaylist("testlist").ShouldEqual(null, "Testsetup is invalid. This Method enters with an existing Playlist with the given name.");

            Playlist plist = pman.CreatePlaylist("testlist");
            pman.AddPlaylist(plist);

            pman.FindPlaylist("testlist").ShouldNotBeNull("After creating and adding a Playlist with th egiven name we were not able to find it.");
            pman.FindPlaylist("testlist").Name.ShouldEqual("testlist", "The found Playlist has not the given name.");
            pman.FindPlaylist("testlist").ShouldEqual(plist, "The found Playlist is not equal to the added Playlist.");
        }

        [Test]
        public void PlaylistManagerLoadPlaylist()
        {
            Playlist plist = pman.CreatePlaylist("testlist");
            Playlist plist2 = pman.CreatePlaylist("testlist2");

            pman.LoadPlaylist(plist);
            pman.LoadedPlaylist.ShouldEqual(plist, "Loaded Playlist does not equal the Playlists that was loaded.");

            pman.LoadPlaylist(plist2);
            pman.LoadedPlaylist.ShouldEqual(plist2, "Loaded Playlist does not equal the Playlists that was loaded.");
        }

        [Test]
        public void PlaylistManagerDeletePlaylist()
        {
            Playlist plist = pman.CreatePlaylist("testlist");

            pman.AddPlaylist(plist);
            pman.Playlists.Count.ShouldEqual(1, "After adding one Playlist Playlists.Count should be 1.");

            pman.DeletePlaylist(plist);
            pman.Playlists.Count.ShouldEqual(0, "After deleten the only Playlist Playlists.Count should be 0.");
        }

        [Test]
        public void PlaylistManagerSavePlaylist()
        {
            pman.SavePlaylist(pman.CreatePlaylist("testlist"));
        }

        [Test]
        public void PlaylistAdd()
        {
            Playlist p = pman.CreatePlaylist("testlist");

            p.Items.Count.ShouldEqual(0, "The Items count of a new Playlist should be 0.");
            p.NextItem.ShouldEqual(null, "The NextItem Property should return null if no Items are available.");

            Listable<MediaItem> lmi = new Listable<MediaItem>(new MediaItem(new Uri("http://validuri.io"), String.Empty));
            p.Add(lmi);

            p.Items.Count.ShouldEqual(1, "After adding one MediaItem the Count should be 1.");
            p.NextItem.ShouldEqual(lmi, "The NextItem Property should return the only MediaItem if available.");
        }

        [Test]
        public void PlaylistRemove()
        {
            Playlist p = pman.CreatePlaylist("testlist");

            p.Items.Count.ShouldEqual(0, "The Items count of a newly created Playlist should be 0.");
            p.NextItem.ShouldEqual(null, "The NextItem Property of a Playlist should return null if no Items are available.");

            Listable<MediaItem> lmi = new Listable<MediaItem>(new MediaItem(new Uri("http://validuri.io"), String.Empty));
            p.Add(lmi);

            p.Items.Count.ShouldEqual(1, "After adding an Item the Count should be 1.");
            p.NextItem.ShouldEqual(lmi, "NextItem should return the just added Item.");

            p.Remove(lmi);

            p.Items.Count.ShouldEqual(0, "After removing the only item on the list count should return 0.");
            p.NextItem.ShouldEqual(null, "The NextItem Property of a Playlist should return null if no Items are available.");
        }

        [Test]
        public void PlaylistNextItem()
        {
            Playlist p = pman.CreatePlaylist("testlist");

            p.Items.Count.ShouldEqual(0, "After creation a Playlist should contain 0 Items.");
            p.NextItem.ShouldEqual(null, "When no Items are available NéxtItem should return null.");

            Listable<MediaItem> lmi1 = new Listable<MediaItem>(new MediaItem(new Uri("http://validuri.io"), String.Empty));
            Listable<MediaItem> lmi2 = new Listable<MediaItem>(new MediaItem(new Uri("http://validuri.io"), String.Empty));
            p.Add(lmi1);
            p.Add(lmi2);

            p.NextItem.ShouldEqual(lmi1, "After adding Items the NextItem Property should return the Items in the order they were added.");
            p.NextItem.ShouldEqual(lmi2, "After adding Items the NextItem Property should return the Items in the order they were added.");
            p.NextItem.ShouldEqual(null, "After returning all Items the NextItem Property should return null.");

            p.ResetAlreadyPlayedItems();

            p.NextItem.ShouldEqual(lmi1, "After resetting the PlayedItems on the Playlist the NextItem Property should return the Items in the order they were added.");
            p.NextItem.ShouldEqual(lmi2, "After resetting the PlayedItems on the Playlist the NextItem Property should return the Items in the order they were added.");
            p.NextItem.ShouldEqual(null, "After returning all Items the NextItem Property should return null.");
        }

        [Test]
        public void PlaylistNextItemRepeatPlaylist()
        {
            Playlist p = pman.CreatePlaylist("testlist");
            IocContainer.Get<IMediaManager>().Repeat = RepeatMode.RepeatPlaylist;

            p.Items.Count.ShouldEqual(0, "After creation a Playlist should contain 0 Items.");
            p.NextItem.ShouldEqual(null, "When no Items are available NextItem should return null.");

            Listable<MediaItem> lmi1 = new Listable<MediaItem>(new MediaItem(new Uri("http://validuri.io"), String.Empty));
            Listable<MediaItem> lmi2 = new Listable<MediaItem>(new MediaItem(new Uri("http://validuri.io"), String.Empty));
            p.Add(lmi1);
            p.Add(lmi2);

            p.NextItem.ShouldEqual(lmi1, "After adding Items the NextItem Property should return the Items in the order they were added.");
            p.NextItem.ShouldEqual(lmi2, "After adding Items the NextItem Property should return the Items in the order they were added.");
            p.NextItem.ShouldEqual(lmi1, "After returning all Items the NextItem Property should return the Items in the order they were added when RepeatMode=Playlist.");
        }

        [Test]
        public void PlaylistNextItemRepeatSong()
        {
            Playlist p = pman.CreatePlaylist("testlist");

            p.Items.Count.ShouldEqual(0, "After creation a Playlist should contain 0 Items.");
            p.NextItem.ShouldEqual(null, "When no Items are available NextItem should return null.");

            Listable<MediaItem> lmi1 = new Listable<MediaItem>(new MediaItem(new Uri("http://validuri.io"), String.Empty));
            Listable<MediaItem> lmi2 = new Listable<MediaItem>(new MediaItem(new Uri("http://validuri.io"), String.Empty));
            p.Add(lmi1);
            p.Add(lmi2);

            p.NextItem.ShouldEqual(lmi1, "After adding Items the NextItem Property should return the Items in the order they were added.");
            IocContainer.Get<IMediaManager>().Repeat = RepeatMode.RepeatSong;
            p.NextItem.ShouldEqual(lmi2, "The NextItem Property should always return the current Item when RepeatMode=Song.");
            p.NextItem.ShouldEqual(lmi2, "The NextItem Property should always return the current Item when RepeatMode=Song.");
        }
    }
}
