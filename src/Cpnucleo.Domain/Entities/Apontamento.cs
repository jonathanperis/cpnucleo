namespace Cpnucleo.Domain.Entities;

public sealed class Apontamento : BaseEntity
{
    public string? Descricao { get; private set; }

    public DateTime DataApontamento { get; private set; }

    public int QtdHoras { get; private set; }

    public Guid IdTarefa { get; private set; }

    public Guid IdRecurso { get; private set; }

    public Tarefa? Tarefa { get; private set; }

    public static Apontamento Create(string descricao, DateTime dataApontamento, int qtdHoras, Guid idTarefa, Guid idRecurso)
    {
        return new Apontamento
        {
            Id = Guid.NewGuid(),
            Descricao = descricao,
            DataApontamento = dataApontamento,
            QtdHoras = qtdHoras,
            IdTarefa = idTarefa,
            IdRecurso = idRecurso,
            DataInclusao = DateTime.UtcNow,
            Ativo = true
        };
    }

    public static Apontamento Update(Apontamento item, string descricao, DateTime dataApontamento, int qtdHoras, Guid idTarefa, Guid idRecurso)
    {
        item.Descricao = descricao;
        item.DataApontamento = dataApontamento;
        item.QtdHoras = qtdHoras;
        item.IdTarefa = idTarefa;
        item.IdRecurso = idRecurso;
        item.DataAlteracao = DateTime.UtcNow;

        return item;
    }

    public static Apontamento Remove(Apontamento item)
    {
        item.Ativo = false;
        item.DataExclusao = DateTime.UtcNow;

        return item;
    }
}
