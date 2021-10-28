using Cpnucleo.Infra.CrossCutting.Util.Events.Sistema;
using Ev.ServiceBus.Reception;

namespace Cpnucleo.Application.Events.Sistema;

public class RemoveSistemaEventHandler : IMessageReceptionHandler<RemoveSistemaEvent>
{
    public Task Handle(RemoveSistemaEvent @event, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
