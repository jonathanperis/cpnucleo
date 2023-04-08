namespace Cpnucleo.Application.Common.Bus;

public interface IEventHandler
{
    Task PublishEventAsync<T>(T @event);
}
