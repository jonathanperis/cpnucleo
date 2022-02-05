namespace Cpnucleo.Application.Commands.Apontamento.UpdateApontamento;

public class UpdateApontamentoCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public string Descricao { get; set; }
    public DateTime DataApontamento { get; set; }
    public int QtdHoras { get; set; }
    public bool Ativo { get; set; }
    public Guid IdTarefa { get; set; }
    public Guid IdRecurso { get; set; }
}
