namespace Cpnucleo.Application.Test.Helpers;

public class EventManagerHelper
{
    public static IEventManager GetInstance(object @object = default)
    {
        Mock<IEventManager> mockServiceBus = new();
        mockServiceBus.Setup(x => x.PublishEventAsync(@object)).Returns(Task.CompletedTask);

        return mockServiceBus.Object;
    }
}
