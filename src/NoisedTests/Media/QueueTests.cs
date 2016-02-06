
using System;
using System.Collections.Generic;
using NUnit.Framework;
using Noised.Core.IOC;
using Noised.Core.Media;
using Should;

namespace NoisedTests.Core.Media
{
    /// <summary>
    ///		Test fixture for testing the media items queue
    /// </summary>
    [TestFixture]
    public class QueueTests
    {
        private IQueue queue;
        private const string TEST_CHECKSUM = "somechecksum";
        private readonly Uri TEST_URI = new Uri("file://something.mp3");

        private Listable<MediaItem> CreateTestItem()
        {
            return new Listable<MediaItem>(new MediaItem(TEST_URI, TEST_CHECKSUM));
        }

        [SetUp]
        public void RunBeforeAnyTests()
        {
            //building configuration object on
            IocContainer.Build();
            queue = IocContainer.Get<IQueue>();
        }

        [Test]
        public void ShouldDequeuedItemIfEnqueued()
        {
            var testItem = CreateTestItem();
            queue.Enqueue(testItem);
            var dequeuedItem = queue.Dequeue();
            dequeuedItem.ShouldEqual(testItem);
            queue.Count.ShouldEqual(0);
        }

        [Test]
        public void ShouldHaveTheCorrectCountWhenEnqueingItems()
        {
            var testItem = CreateTestItem();
            queue.Enqueue(testItem);
            queue.Count.ShouldEqual(1);
            queue.Dequeue();
            queue.Count.ShouldEqual(0);
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

            queue.Remove(testItem.ItemID);

            //The first item in the queue should now be the 2nd created
            var dequeuedItem = queue.Dequeue();
            dequeuedItem.ShouldEqual(testItem2);
            queue.Clear();
            queue.Count.ShouldEqual(0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ShuldNotAllowToEnqueueAnListItemMoreThanOnce()
        {
            var testItem = CreateTestItem();
            queue.Enqueue(testItem);
            queue.Enqueue(testItem);
            queue.Clear();
            queue.Count.ShouldEqual(0);
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
        }
    };
}
