namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.Apontamento;

public class UpdateApontamentoCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public string Descricao { get; set; }
    public DateTime DataApontamento { get; set; }
    public int QtdHoras { get; set; }
    public Guid IdTarefa { get; set; }
    public Guid IdRecurso { get; set; }
}
