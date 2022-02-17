namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento;

public class UpdateApontamentoCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public string Descricao { get; set; }
    public DateTime DataApontamento { get; set; }
    public int QtdHoras { get; set; }
    public Guid IdTarefa { get; set; }
    public Guid IdRecurso { get; set; }
}
