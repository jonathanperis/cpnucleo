namespace Cpnucleo.Application.Common.Bus.Interfaces;

public interface IEventHandler
{
    Task PublishEventAsync<T>(T @event);
}
