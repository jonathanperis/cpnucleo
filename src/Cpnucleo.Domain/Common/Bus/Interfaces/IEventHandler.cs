namespace Cpnucleo.Domain.Common.Bus.Interfaces;

public interface IEventHandler
{
    Task PublishEventAsync<T>(T @event);
}
