namespace Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa;

public class UpdateImpedimentoTarefaCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public string Descricao { get; set; }
    public Guid IdTarefa { get; set; }
    public Guid IdImpedimento { get; set; }
}
