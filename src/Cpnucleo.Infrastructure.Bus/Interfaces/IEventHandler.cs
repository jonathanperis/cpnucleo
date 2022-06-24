namespace Cpnucleo.Infrastructure.Bus.Interfaces;

public interface IEventHandler
{
    Task PublishEventAsync<T>(T @event);
}
