namespace Cpnucleo.Domain.Entities;

public class RecursoProjeto : BaseEntity
{
    public Guid IdRecurso { get; set; }

    public Guid IdProjeto { get; set; }

    public Recurso? Recurso { get; set; }

    public Projeto? Projeto { get; set; }
}
