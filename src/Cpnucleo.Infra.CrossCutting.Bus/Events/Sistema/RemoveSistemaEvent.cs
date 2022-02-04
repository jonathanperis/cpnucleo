namespace Cpnucleo.Infra.CrossCutting.Bus.Events.Sistema;

public class RemoveSistemaEvent
{
    public RemoveSistemaEvent(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}
