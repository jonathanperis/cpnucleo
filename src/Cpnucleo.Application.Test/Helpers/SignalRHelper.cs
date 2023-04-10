namespace Cpnucleo.Application.Test.Helpers;

public class SignalRHelper
{
    public static IHubContext<ApplicationHub> GetInstance()
    {
        Mock<IClientProxy> mockClientProxy = new();

        Mock<IHubClients> mockClients = new();
        mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);

        Mock<IHubContext<ApplicationHub>> mockHub = new();
        mockHub.Setup(x => x.Clients).Returns(() => mockClients.Object);

        return mockHub.Object;
    }
}
