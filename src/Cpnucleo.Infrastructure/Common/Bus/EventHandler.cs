using Cpnucleo.Application.Common.Bus;
using Ev.ServiceBus.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Cpnucleo.Infrastructure.Common.Bus;

internal sealed class EventHandler : IEventHandler
{
    private readonly IServiceProvider _serviceProvider;

    public EventHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task PublishEventAsync<T>(T @event)
    {
        IMessagePublisher publisher = _serviceProvider.GetService<IMessagePublisher>();
        publisher.Publish(@event);

        IMessageDispatcher dispatcher = _serviceProvider.GetService<IMessageDispatcher>();
        await dispatcher.ExecuteDispatches();
    }
}
