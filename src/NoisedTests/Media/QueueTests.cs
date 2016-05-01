using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Noised.Core.Commands;
using Noised.Core.Media;
using Noised.Core.Service;
using Should;

namespace NoisedTests.Core.Media
{
    /// <summary>
    ///	Test fixture for testing the media items queue
    /// </summary>
    [TestFixture]
    public class QueueTests
    {
        private IQueue queue;
        private Mock<IServiceConnectionManager> connectionManager;
        private const string TEST_CHECKSUM = "somechecksum";
        private readonly Uri TEST_URI = new Uri("file://something.mp3");

        private Listable<MediaItem> CreateTestItem()
        {
            return new Listable<MediaItem>(new MediaItem(TEST_URI, TEST_CHECKSUM));
        }

        private void VerifyBroadcastCalled(int times)
        {
            const string expectedResponseName = "Noised.Commands.Core.GetQueue";
            connectionManager.Verify(cm => cm.SendBroadcast(It.Is<ResponseMetaData>(m => m.Name == expectedResponseName)), Times.Exactly(times));
        }

        [SetUp]
        public void SetupTest()
        {
            connectionManager = new Mock<IServiceConnectionManager>();
            queue = new Queue(connectionManager.Object);
        }

        [Test]
        public void ShouldDequeueTheSammeItemWhichWasEnqueued()
        {
            var testItem = CreateTestItem();
            queue.Enqueue(testItem);
            var dequeuedItem = queue.Dequeue();
            dequeuedItem.ShouldEqual(testItem);
            queue.Count.ShouldEqual(0);
            VerifyBroadcastCalled(2);
        }

        [Test]
        public void ShouldHaveTheCorrectCountWhenEnqueingItems()
        {
            var testItem = CreateTestItem();
            queue.Enqueue(testItem);
            queue.Count.ShouldEqual(1);
            queue.Dequeue();
            queue.Count.ShouldEqual(0);
            VerifyBroadcastCalled(2);
        }

        [Test]
        public void ShouldBeEmptyAfterCallingClear()
        {
            queue.Enqueue(CreateTestItem());
            queue.Enqueue(CreateTestItem());
            queue.Enqueue(CreateTestItem());
            queue.Enqueue(CreateTestItem());
            queue.Count.ShouldEqual(4);
            queue.Clear();
            queue.Count.ShouldEqual(0);
            VerifyBroadcastCalled(5);
        }

        [Test]
        public void ShouldRemoveCorrectItemWhenCallingRemove()
        {
            var testItem = CreateTestItem();
            queue.Enqueue(testItem);
            queue.Count.ShouldEqual(1);

            var testItem2 = CreateTestItem();
            queue.Enqueue(testItem2);
            queue.Count.ShouldEqual(2);

            queue.Remove(testItem.ListId);

            //The first item in the queue should now be the 2nd created
            var dequeuedItem = queue.Dequeue();
            dequeuedItem.ShouldEqual(testItem2);
            queue.Clear();
            queue.Count.ShouldEqual(0);

            VerifyBroadcastCalled(5);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ShuldNotAllowToEnqueueAnListItemMoreThanOnce()
        {
            //While it is possible to enqueue a MediaItem more than once, a Listable item must be unique 
            var testItem = CreateTestItem();
            queue.Enqueue(testItem);
            queue.Enqueue(testItem);
            queue.Clear();
            queue.Count.ShouldEqual(0);
            VerifyBroadcastCalled(3);
        }

        [Test]
        public void ShouldEnqueueAListOfItems()
        {
            var list = new List<Listable<MediaItem>>
            {
                CreateTestItem(),
                CreateTestItem(),
                CreateTestItem(),
                CreateTestItem(),
            };
            queue.Enqueue(list);
            queue.Count.ShouldEqual(list.Count);
            queue.Clear();
            queue.Count.ShouldEqual(0);
            VerifyBroadcastCalled(2);
        }

        [Test]
        public void ShouldRemoveAnItem()
        {
            var item1 = CreateTestItem();
            var item2 = CreateTestItem();
            queue.Enqueue(item1);
            queue.Enqueue(item2);
            queue.Count.ShouldEqual(2);
            queue.Remove(item1.ListId);
            queue.Count.ShouldEqual(1);
            var itemInQueue = queue.Dequeue();
            itemInQueue.ListId.ShouldEqual(item2.ListId);
            VerifyBroadcastCalled(4);
        }
    };
}
