namespace Cpnucleo.Application.Commands.ImpedimentoTarefa.CreateImpedimentoTarefa;

public class CreateImpedimentoTarefaCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public string Descricao { get; set; }
    public Guid IdTarefa { get; set; }
    public Guid IdImpedimento { get; set; }
}
