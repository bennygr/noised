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
//    public class RepeatModeTest
//    {
//        //[TestFixtureSetUp]
//        //public void RepeatModeTestSetUp()
//        //{
//        //    IoC.Build();
//        //}
//
//        //[Test]
//        //public void GetRepeatMode()
//        //{
//        //    IMediaManager mediaManager = IoC.Get<IMediaManager>();
//
//        //    ResponseMetaData responseMetaData = new ResponseMetaData();
//        //    Mock<IServiceConnectionContext> serviceConnectionMock = new Mock<IServiceConnectionContext>();
//        //    serviceConnectionMock.Setup(x => x.SendResponse(It.IsAny<ResponseMetaData>())).Callback((ResponseMetaData r) => responseMetaData = r);
//
//        //    new GetRepeatMode(serviceConnectionMock.Object).ExecuteCommand();
//
//        //    responseMetaData.Parameters[0].ShouldEqual(mediaManager.Repeat, "GetRepeatMode command should return the IMediaManagers repeat mode.");
//        //}
//
//        //[Test]
//        //[ExpectedException(typeof(ArgumentNullException), UserMessage = "GetRepeatMode command should throw an ArgumentNullException when invoked without IServiceConnectionContext.")]
//        //public void GetRepeatModeArgumentNullException()
//        //{
//        //    new GetRepeatMode(null);
//        //}
//
//        //[Test]
//        //public void SetRepeatMode()
//        //{
//        //    IMediaManager mediaManager = IoC.Get<IMediaManager>();
//
//        //    Mock<IServiceConnectionContext> serviceConnectionMock = new Mock<IServiceConnectionContext>();
//
//        //    new SetRepeatMode(serviceConnectionMock.Object, "RepeatSong").ExecuteCommand();
//        //    mediaManager.Repeat.ShouldEqual(RepeatMode.RepeatSong);
//
//        //    new SetRepeatMode(serviceConnectionMock.Object, "RepeatPlaylist").ExecuteCommand();
//        //    mediaManager.Repeat.ShouldEqual(RepeatMode.RepeatPlaylist);
//
//        //    new SetRepeatMode(serviceConnectionMock.Object, "None").ExecuteCommand();
//        //    mediaManager.Repeat.ShouldEqual(RepeatMode.None);
//        //}
//
//        //[Test]
//        //[ExpectedException(typeof (ArgumentException), UserMessage = "Trying to set an invalid RepeatMode should result in an ArgumentException.")]
//        //public void SetRepeatModeArgumentException()
//        //{
//        //    Mock<IServiceConnectionContext> serviceConnectionMock = new Mock<IServiceConnectionContext>();
//        //    new SetRepeatMode(serviceConnectionMock.Object, "InvalidRepeatMode");
//        //}
//
//        //[Test]
//        //[ExpectedException(typeof (ArgumentNullException),
//        //    UserMessage =
//        //        "SetRepeatMode command should throw an ArgumentNullException when invoked without IServiceConnectionContext."
//        //    )]
//        //public void SetRepeatModeArgumentNullException()
//        //{
//        //    new SetRepeatMode(null, "None");
//        //}
//    }
//}
