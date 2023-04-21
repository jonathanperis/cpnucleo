namespace Cpnucleo.Infrastructure.Common.Bus;

internal sealed class EventManager : IEventManager
{
    private readonly IServiceProvider _serviceProvider;

    public EventManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task PublishEventAsync<T>(T @event)
    {
        var publisher = _serviceProvider.GetService<IMessagePublisher>();
        publisher?.Publish(@event);

        var dispatcher = _serviceProvider.GetService<IMessageDispatcher>();
        await dispatcher?.ExecuteDispatches()!;
    }
}
