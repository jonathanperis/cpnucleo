namespace Cpnucleo.Domain.Entities;

public sealed class Workflow : BaseEntity
{
    public string? Nome { get; private set; }

    public int Ordem { get; private set; }

    public static Workflow Create(string nome, int ordem, Guid id = default)
    {
        return new Workflow
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id,
            Nome = nome,
            Ordem = ordem,
            DataInclusao = DateTime.UtcNow,
            Ativo = true
        };
    }

    public static Workflow Update(Workflow item, string nome, int ordem)
    {
        item.Nome = nome;
        item.Ordem = ordem;
        item.DataAlteracao = DateTime.UtcNow;

        return item;
    }

    public static Workflow Remove(Workflow item)
    {
        item.Ativo = false;
        item.DataExclusao = DateTime.UtcNow;

        return item;
    }

    public static string GetTamanhoColuna(int colunas)
    {
        colunas = colunas == 1 ? 2 : colunas;

        int i = 12 / colunas;
        return i.ToString();
    }
}
