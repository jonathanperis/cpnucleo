namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.Tarefa;

public class RemoveTarefaCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
}
