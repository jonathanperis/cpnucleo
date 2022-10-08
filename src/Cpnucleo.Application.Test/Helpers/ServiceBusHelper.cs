using Cpnucleo.Domain.Common.Bus.Interfaces;
using Moq;

namespace Cpnucleo.Application.Test.Helpers;

public class ServiceBusHelper
{
    public static Mock<IEventHandler> GetInstance(object @object = default)
    {
        Mock<IEventHandler> mockServiceBus = new();
        mockServiceBus.Setup(x => x.PublishEventAsync(@object)).Returns(Task.CompletedTask);

        return mockServiceBus;
    }
}
