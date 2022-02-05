namespace Cpnucleo.Application.Commands.Apontamento.CreateApontamento;

public class CreateApontamentoCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public string Descricao { get; set; }
    public DateTime DataApontamento { get; set; }
    public int QtdHoras { get; set; }
    public Guid IdTarefa { get; set; }
    public Guid IdRecurso { get; set; }
}
