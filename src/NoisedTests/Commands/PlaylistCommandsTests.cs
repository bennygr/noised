//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Moq;
//using Noised.Core;
//using Noised.Core.Commands;
//using Noised.Core.DB;
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
//    public class PlaylistCommandsTests
//    {
//        //private IPlaylistManager playlistManager;
//
//        //[TestFixtureSetUp]
//        //public void PlaylistCommandsTestsFixtureSetUp()
//        //{
//        //    IocContainer.Build();
//
//        //    Mock<IPlaylistRepository> mockPlaylistRepository = new Mock<IPlaylistRepository>();
//        //    mockPlaylistRepository.Setup(x => x.GetAll()).Returns(new List<Playlist>());
//
//        //    Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
//        //    mockUnitOfWork.Setup(x => x.PlaylistRepository).Returns(mockPlaylistRepository.Object);
//
//        //    Mock<IDbFactory> mockFactory = new Mock<IDbFactory>();
//        //    mockFactory.Setup(x => x.GetUnitOfWork()).Returns(mockUnitOfWork.Object);
//
//        //    playlistManager = IocContainer.Get<IPlaylistManager>();
//        //}
//
//        //[Test]
//        //public void CreatePlaylistCommandTest()
//        //{
//        //    var diContainer = new Mock<IDIContainer>(); 
//        //    var context = new Mock<IServiceConnectionContext>();
//        //    context.Setup(c => c.DIContainer).Returns(diContainer.Object);
//
//        //    new CreatePlaylist(context.Object,"CreatePlaylistCommandTest").ExecuteCommand();
//        //    context.Verify(m => m.)
//
//        //    
//        //    playlistManager.Playlists.Count.ShouldEqual(initialCount + 1);
//        //    playlistManager.Playlists.ToList().Find(x => x.Name == "CreatePlaylistCommandTest").ShouldNotBeNull();
//        //    playlistManager.Playlists.ToList().Find(x => x.Name == "CreatePlaylistCommandTest").Items.Count.ShouldEqual(0);
//        //}
//
//        //[Test]
//        //[ExpectedException(typeof(ArgumentNullException))]
//        //public void CreatePlaylistCommandArgumentNullExceptionTest()
//        //{
//        //    new CreatePlaylist(null, "CreatePlaylistCommandTest").ExecuteCommand();
//        //}
//
//        //[Test]
//        //public void CreatePlaylistCommandArgumentExceptionTest()
//        //{
//        //    ResponseMetaData responseMetaData = new ResponseMetaData();
//        //    Mock<IServiceConnectionContext> serviceConnectionMock = new Mock<IServiceConnectionContext>();
//        //    serviceConnectionMock.Setup(x => x.SendResponse(It.IsAny<ResponseMetaData>())).Callback((ResponseMetaData r) => responseMetaData = r);
//
//        //    try
//        //    {
//        //        new CreatePlaylist(serviceConnectionMock.Object, String.Empty).ExecuteCommand();
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        e.ShouldBeType<ArgumentException>();
//        //    }
//
//        //    responseMetaData.Name.ShouldEqual("Noised.Commands.Core.CreatePlaylist");
//        //    responseMetaData.Parameters.Count.ShouldEqual(1);
//        //    responseMetaData.Parameters[0].ToString().ShouldStartWith(strings.NoValidPlaylistName);
//        //}
//
//        //[Test]
//        //public void AddToPlaylistCommandTest()
//        //{
//        //    playlistManager.AddPlaylist(playlistManager.CreatePlaylist("AddToPlaylistCommand"));
//
//        //    new AddToPlaylist(new Mock<IServiceConnectionContext>().Object, "AddToPlaylistCommand",
//        //        new List<object> { "http://first.uri", "http://second.uri" }).ExecuteCommand();
//
//        //    playlistManager.Playlists.ToList().Find(x => x.Name == "AddToPlaylistCommand").Items.Count.ShouldEqual(2);
//        //}
//
//        //[Test]
//        //public void AddToPlaylistCommandArgumentExceptionTest()
//        //{
//        //    ResponseMetaData responseMetaData = new ResponseMetaData();
//        //    Mock<IServiceConnectionContext> serviceConnectionMock = new Mock<IServiceConnectionContext>();
//        //    serviceConnectionMock.Setup(x => x.SendResponse(It.IsAny<ResponseMetaData>())).Callback((ResponseMetaData r) => responseMetaData = r);
//
//        //    try
//        //    {
//        //        new AddToPlaylist(serviceConnectionMock.Object, String.Empty, new List<object>()).ExecuteCommand();
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        e.ShouldBeType<ArgumentException>();
//        //    }
//
//        //    responseMetaData.Name.ShouldEqual("Noised.Commands.Core.AddToPlaylist");
//        //    responseMetaData.Parameters.Count.ShouldEqual(1);
//        //    responseMetaData.Parameters[0].ToString().ShouldStartWith(strings.NoValidPlaylistName);
//        //}
//
//        //[Test]
//        //public void AddToPlaylistCommandArgumentNullExceptionTest()
//        //{
//        //    ResponseMetaData responseMetaData = new ResponseMetaData();
//        //    Mock<IServiceConnectionContext> serviceConnectionMock = new Mock<IServiceConnectionContext>();
//        //    serviceConnectionMock.Setup(x => x.SendResponse(It.IsAny<ResponseMetaData>())).Callback((ResponseMetaData r) => responseMetaData = r);
//
//        //    try
//        //    {
//        //        new AddToPlaylist(serviceConnectionMock.Object, "testlist", new List<object>()).ExecuteCommand();
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        e.ShouldBeType<ArgumentNullException>();
//        //    }
//
//        //    responseMetaData.Name.ShouldEqual("Noised.Commands.Core.AddToPlaylist");
//        //    responseMetaData.Parameters.Count.ShouldEqual(1);
//        //    responseMetaData.Parameters[0].ToString().ShouldContain("mediaItemUris");
//        //}
//
//        //[Test]
//        //public void DeletePlaylistCommandTest()
//        //{
//        //    Playlist p = playlistManager.Playlists.ToList().Find(x => x.Name == "testlist");
//
//        //    if (p == null)
//        //        playlistManager.AddPlaylist(playlistManager.CreatePlaylist("testlist"));
//
//        //    new DeletePlaylist(new Mock<IServiceConnectionContext>().Object, "testlist").ExecuteCommand();
//
//        //    playlistManager.Playlists.ToList().Find(x => x.Name == "testlist").ShouldBeNull();
//        //}
//
//        //[Test]
//        //[ExpectedException(typeof(ArgumentNullException))]
//        //public void DeletePlaylistCommandArgumentNullExceptionTest()
//        //{
//        //    new DeletePlaylist(null, String.Empty).ExecuteCommand();
//        //}
//
//        //[Test]
//        //public void DeletePlaylistCommandArgumentExceptionTest()
//        //{
//        //    ResponseMetaData responseMetaData = new ResponseMetaData();
//        //    Mock<IServiceConnectionContext> serviceConnectionMock = new Mock<IServiceConnectionContext>();
//        //    serviceConnectionMock.Setup(x => x.SendResponse(It.IsAny<ResponseMetaData>())).Callback((ResponseMetaData r) => responseMetaData = r);
//
//        //    try
//        //    {
//        //        new DeletePlaylist(serviceConnectionMock.Object, String.Empty).ExecuteCommand();
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        e.ShouldBeType<ArgumentException>();
//        //    }
//
//        //    responseMetaData.Name.ShouldEqual("Noised.Commands.Core.DeletePlaylist");
//        //    responseMetaData.Parameters.Count.ShouldEqual(1);
//        //    responseMetaData.Parameters[0].ToString().ShouldContain("playlistName");
//        //}
//
//        //[Test]
//        //public void RemoveFromPlaylistTest()
//        //{
//
//        //}
//
//        //[Test]
//        //[ExpectedException(typeof(ArgumentNullException))]
//        //public void RemoveFromPlaylistArgumentNullExceptionTest()
//        //{
//        //    new RemoveFromPlaylist(null, String.Empty, new List<string>()).ExecuteCommand();
//        //}
//
//        //[Test]
//        //public void RemoveFromPlaylistArgumentExceptionTest()
//        //{
//        //    ResponseMetaData responseMetaData = new ResponseMetaData();
//        //    Mock<IServiceConnectionContext> serviceConnectionMock = new Mock<IServiceConnectionContext>();
//        //    serviceConnectionMock.Setup(x => x.SendResponse(It.IsAny<ResponseMetaData>())).Callback((ResponseMetaData r) => responseMetaData = r);
//
//        //    try
//        //    {
//        //        new RemoveFromPlaylist(serviceConnectionMock.Object, String.Empty, new List<string> { "http://uri.uri" }).ExecuteCommand();
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        e.ShouldBeType<ArgumentException>();
//        //    }
//
//        //    responseMetaData.Name.ShouldEqual("Noised.Plugins.Commands.CoreCommands.RemoveFromPlaylist");
//        //    responseMetaData.Parameters.Count.ShouldEqual(1);
//        //    responseMetaData.Parameters[0].ToString().ShouldStartWith(strings.NoValidPlaylistName);
//        //}
//
//        //[Test]
//        //public void RemoveFromPlaylistArgumentNullExceptionTest2()
//        //{
//        //    ResponseMetaData responseMetaData = new ResponseMetaData();
//        //    Mock<IServiceConnectionContext> serviceConnectionMock = new Mock<IServiceConnectionContext>();
//        //    serviceConnectionMock.Setup(x => x.SendResponse(It.IsAny<ResponseMetaData>())).Callback((ResponseMetaData r) => responseMetaData = r);
//
//        //    try
//        //    {
//        //        new RemoveFromPlaylist(serviceConnectionMock.Object, "testlist", new List<string>()).ExecuteCommand();
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        e.ShouldBeType<ArgumentNullException>();
//        //    }
//
//        //    responseMetaData.Name.ShouldEqual("Noised.Plugins.Commands.CoreCommands.RemoveFromPlaylist");
//        //    responseMetaData.Parameters.Count.ShouldEqual(1);
//        //    responseMetaData.Parameters[0].ToString().ShouldContain("please provide valid MediaItemUris");
//        //}
//    }
//}
