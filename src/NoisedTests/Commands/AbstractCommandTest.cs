using Moq;
using NUnit.Framework;
using Noised.Core.IOC;
using Noised.Core.Service;

/// <summary>
///	Abstract class for all command tests
/// </summary>
public class AbstractCommandTest
{
    private Mock<IServiceConnectionContext> contextMock;
    private Mock<IDIContainer> iocMock;

    /// <summary>
    ///	    The command context mock which can be used for executing commands from within unit tests 
    /// </summary>
    protected Mock<IServiceConnectionContext> ContextMock
    {
        get
        {
            return contextMock;
        }
    }

    /// <summary>
    ///	    Constructor
    /// </summary>
    [TestFixtureSetUp]
    public void Setup()
    {
        iocMock = new Mock<IDIContainer>();
        contextMock = new Mock<IServiceConnectionContext>();
        contextMock.Setup(c => c.DIContainer).Returns(iocMock.Object);
    }

    /// <summary>
    ///	    Rgisters a dependecy of type {{T}} to the mocked DIContainer 
    /// </summary>
    /// <param name="dependency">The dependency object to register for type {{T}}</param>
    protected void RegisterToDIMock<T>(T dependency)
    {
        iocMock.Setup(ioc => ioc.Get<T>()).Returns(dependency);
    }
};
