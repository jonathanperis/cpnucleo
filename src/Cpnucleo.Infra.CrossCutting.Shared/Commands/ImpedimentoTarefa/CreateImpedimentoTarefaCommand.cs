namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.ImpedimentoTarefa;

public class CreateImpedimentoTarefaCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public string Descricao { get; set; }
    public Guid IdTarefa { get; set; }
    public Guid IdImpedimento { get; set; }
}
