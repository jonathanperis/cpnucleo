using Cpnucleo.Application.Hubs;
using Microsoft.AspNetCore.SignalR;
using Moq;

namespace Cpnucleo.Application.Test.Helpers;

public class SignalRHelper
{
    public static Mock<IHubContext<CpnucleoHub>> GetInstance()
    {
        Mock<IClientProxy> mockClientProxy = new();

        Mock<IHubClients> mockClients = new();
        mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);

        Mock<IHubContext<CpnucleoHub>> mockHub = new();
        mockHub.Setup(x => x.Clients).Returns(() => mockClients.Object);

        return mockHub;
    }
}
