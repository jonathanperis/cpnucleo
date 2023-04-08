namespace Cpnucleo.Domain.Entities;

public sealed class RecursoProjeto : BaseEntity
{
    public Guid IdRecurso { get; private set; }

    public Guid IdProjeto { get; private set; }

    public Recurso? Recurso { get; private set; }

    public Projeto? Projeto { get; private set; }

    public static RecursoProjeto Create(Guid idRecurso, Guid idProjeto, Guid id = default)
    {
        return new RecursoProjeto
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id,
            IdRecurso = idRecurso,
            IdProjeto = idProjeto,
            DataInclusao = DateTime.UtcNow,
            Ativo = true
        };
    }

    public static RecursoProjeto Update(RecursoProjeto item, Guid idRecurso, Guid idProjeto)
    {
        item.IdRecurso = idRecurso;
        item.IdProjeto = idProjeto;
        item.DataAlteracao = DateTime.UtcNow;

        return item;
    }

    public static RecursoProjeto Remove(RecursoProjeto item)
    {
        item.Ativo = false;
        item.DataExclusao = DateTime.UtcNow;

        return item;
    }
}
