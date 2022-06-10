namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.ImpedimentoTarefa;

public class RemoveImpedimentoTarefaCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
}
