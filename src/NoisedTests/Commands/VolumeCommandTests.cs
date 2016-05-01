//using Moq;
//using Noised.Core;
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
//    public class VolumeCommandTests
//    {
//        //[TestFixtureSetUp]
//        //public void VolumeTestSetUp()
//        //{
//        //    IoC.Build();
//        //}
//
//        //[Test]
//        //public void SetVolumeTest()
//        //{
//        //    var serviceConnectionMock = new Mock<IServiceConnectionContext>();
// 
//        //    IoC.Get<ICore>().ExecuteCommand(new SetVolume(serviceConnectionMock.Object, 50));
//
//        //    IoC.Get<IMediaManager>().Volume.ShouldEqual(50);
//
//        //    IoC.Get<ICore>().ExecuteCommand(new SetVolume(serviceConnectionMock.Object, 42));
//
//        //    IoC.Get<IMediaManager>().Volume.ShouldEqual(42);
//
//        //    IoC.Get<ICore>().ExecuteCommand(new SetVolume(serviceConnectionMock.Object, 101));
//
//        //    IoC.Get<IMediaManager>().Volume.ShouldEqual(100);
//        //}
//
//        //[Test]
//        //public void GetVolumeTest()
//        //{
//        //    var responseMetaData = new ResponseMetaData();
//        //    var serviceConnectionMock = new Mock<IServiceConnectionContext>();
//        //    serviceConnectionMock.Setup(x => x.SendResponse(It.IsAny<ResponseMetaData>())).Callback((ResponseMetaData r) => responseMetaData = r);
//
//        //    new GetVolume(serviceConnectionMock.Object).ExecuteCommand();
//
//        //    responseMetaData.Parameters[0].ShouldEqual(50);
//
//        //    IoC.Get<IMediaManager>().Volume = 66;
//
//        //    new GetVolume(serviceConnectionMock.Object).ExecuteCommand();
//
//        //    responseMetaData.Parameters[0].ShouldEqual(66);
//        //}
//    }
//}
