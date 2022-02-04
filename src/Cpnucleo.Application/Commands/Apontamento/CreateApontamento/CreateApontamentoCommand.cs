namespace Cpnucleo.Application.Commands.Apontamento.CreateApontamento;

public class CreateApontamentoCommand : IRequest<OperationResult>
{
    public CreateApontamentoCommand(Guid id, string descricao, DateTime dataApontamento, int qtdHoras, Guid idTarefa, Guid idRecurso)
    {
        Id = id;
        Descricao = descricao;
        DataApontamento = dataApontamento;
        QtdHoras = qtdHoras;
        IdTarefa = idTarefa;
        IdRecurso = idRecurso;
    }

    public Guid Id { get; }
    public string Descricao { get; }
    public DateTime DataApontamento { get; }
    public int QtdHoras { get; }
    public Guid IdTarefa { get; }
    public Guid IdRecurso { get; }
}
