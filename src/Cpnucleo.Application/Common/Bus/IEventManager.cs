namespace Cpnucleo.Application.Common.Bus;

public interface IEventManager
{
    Task PublishEventAsync<T>(T @event);
}
