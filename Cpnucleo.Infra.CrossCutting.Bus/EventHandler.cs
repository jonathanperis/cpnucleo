using Cpnucleo.Infra.CrossCutting.Bus.Interfaces;
using Ev.ServiceBus.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Cpnucleo.Infra.CrossCutting.Bus;

public class EventHandler : IEventHandler
{
    private readonly IServiceProvider _serviceProvider;

    public EventHandler(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public async Task PublishEventAsync<T>(T @event)
    {
        var publisher = _serviceProvider.GetService<IMessagePublisher>();
        publisher.Publish(@event);

        var dispatcher = _serviceProvider.GetService<IMessageDispatcher>();
        await dispatcher.ExecuteDispatches();
    }
}
