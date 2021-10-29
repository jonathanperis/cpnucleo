namespace Cpnucleo.Infra.CrossCutting.Bus.Interfaces;

public interface IEventHandler
{
    Task PublishEventAsync<T>(T @event);
}
