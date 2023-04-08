namespace Cpnucleo.Domain.Entities;

public sealed class ImpedimentoTarefa : BaseEntity
{
    public string? Descricao { get; private set; }

    public Guid IdTarefa { get; private set; }

    public Guid IdImpedimento { get; private set; }

    public Tarefa? Tarefa { get; private set; }

    public Impedimento? Impedimento { get; private set; }

    public static ImpedimentoTarefa Create(string descricao, Guid idTarefa, Guid idImpedimento, Guid id = default)
    {
        return new ImpedimentoTarefa
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id,
            Descricao = descricao,
            IdTarefa = idTarefa,
            IdImpedimento = idImpedimento,
            DataInclusao = DateTime.UtcNow,
            Ativo = true
        };
    }

    public static ImpedimentoTarefa Update(ImpedimentoTarefa item, string descricao, Guid idTarefa, Guid idImpedimento)
    {
        item.Descricao = descricao;
        item.IdTarefa = idTarefa;
        item.IdImpedimento = idImpedimento;
        item.DataAlteracao = DateTime.UtcNow;

        return item;
    }

    public static ImpedimentoTarefa Remove(ImpedimentoTarefa item)
    {
        item.Ativo = false;
        item.DataExclusao = DateTime.UtcNow;

        return item;
    }
}
