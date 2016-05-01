//using System;
//using Moq;
//using Noised.Core.Commands;
//using Noised.Core.IOC;
//using Noised.Core.Media;
//using Noised.Core.Service;
//using Noised.Plugins.Commands.CoreCommands;
//using NUnit.Framework;
//using Should;
//
//namespace NoisedTests.Commands
//{
//    [TestFixture]
//    public class ShuffleCommandsTest
//    {
//        //[TestFixtureSetUp]
//        //public void ShuffleStatusTestSetUp()
//        //{
//        //    IoC.Build();
//        //}
//
//        //[Test]
//        //public void SetShuffleStatusSwitchShuffleStatus()
//        //{
//        //    Mock<IServiceConnectionContext> serviceConnectionMock = new Mock<IServiceConnectionContext>();
//
//        //    IMediaManager mediaManager = IoC.Get<IMediaManager>();
//
//        //    new SetShuffleStatus(serviceConnectionMock.Object, false).ExecuteCommand();
//        //    mediaManager.Shuffle.ShouldBeFalse("ShuffleStatus should be false after executing SetShuffleStatus command with parameter false.");
//        //    new SetShuffleStatus(serviceConnectionMock.Object, true).ExecuteCommand();
//        //    mediaManager.Shuffle.ShouldBeTrue("ShuffleStatus should be true after executing SetShuffleStatus command with parameter true.");
//        //}
//
//        //[Test]
//        //[ExpectedException(typeof(ArgumentNullException), UserMessage = "SetShuffleStatus command should throw an ArgumentNullException when invoked without IServiceConnectionContext.")]
//        //public void SetShuffleStatusArgumentNullException()
//        //{
//        //    new SetShuffleStatus(null, false);
//        //}
//
//        //[Test]
//        //public void GetShuffleStatusAfterSwitch()
//        //{
//        //    IMediaManager mediaManager = IoC.Get<IMediaManager>();
//
//        //    ResponseMetaData responseMetaData = new ResponseMetaData();
//        //    Mock<IServiceConnectionContext> serviceConnectionMock = new Mock<IServiceConnectionContext>();
//        //    serviceConnectionMock.Setup(x => x.SendResponse(It.IsAny<ResponseMetaData>())).Callback((ResponseMetaData r) => responseMetaData = r);
//
//        //    new GetShuffleStatus(serviceConnectionMock.Object).ExecuteCommand();
//
//        //    responseMetaData.Parameters[0].ShouldEqual(mediaManager.Shuffle, "GetShuffleStatus command should return the IMediaManagers shuffle status.");
//        //}
//
//        //[Test]
//        //[ExpectedException(typeof(ArgumentNullException), UserMessage = "GetShuffleStatus command should throw an ArgumentNullException when invoked without IServiceConnectionContext.")]
//        //public void GetShuffleStatusArgumentNullException()
//        //{
//        //    new GetShuffleStatus(null);
//        //}
//    }
//}
