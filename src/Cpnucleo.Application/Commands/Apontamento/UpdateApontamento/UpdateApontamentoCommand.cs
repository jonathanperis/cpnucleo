namespace Cpnucleo.Application.Commands.Apontamento.UpdateApontamento;

public class UpdateApontamentoCommand : IRequest<OperationResult>
{
    public UpdateApontamentoCommand(Guid id, string descricao, DateTime dataApontamento, int qtdHoras, bool ativo, Guid idTarefa, Guid idRecurso)
    {
        Id = id;
        Descricao = descricao;
        DataApontamento = dataApontamento;
        QtdHoras = qtdHoras;
        Ativo = ativo;
        IdTarefa = idTarefa;
        IdRecurso = idRecurso;
    }

    public Guid Id { get; }
    public string Descricao { get; }
    public DateTime DataApontamento { get; }
    public int QtdHoras { get; }
    public bool Ativo { get; }
    public Guid IdTarefa { get; }
    public Guid IdRecurso { get; }
}
