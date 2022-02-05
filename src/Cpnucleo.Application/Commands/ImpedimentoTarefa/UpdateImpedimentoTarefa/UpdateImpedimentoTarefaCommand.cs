namespace Cpnucleo.Application.Commands.ImpedimentoTarefa.UpdateImpedimentoTarefa;

public class UpdateImpedimentoTarefaCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public string Descricao { get; set; }
    public Guid IdTarefa { get; set; }
    public Guid IdImpedimento { get; set; }
    public bool Ativo { get; set; }
}
