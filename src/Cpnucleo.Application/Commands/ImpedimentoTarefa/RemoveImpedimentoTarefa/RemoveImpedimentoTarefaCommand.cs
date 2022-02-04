namespace Cpnucleo.Application.Commands.ImpedimentoTarefa.RemoveImpedimentoTarefa;

public class RemoveImpedimentoTarefaCommand : IRequest<OperationResult>
{
    public Guid Id { get; }

    public RemoveImpedimentoTarefaCommand(Guid id)
    {
        Id = id;
    }
}
