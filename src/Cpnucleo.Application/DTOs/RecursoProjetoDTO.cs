namespace Cpnucleo.Application.DTOs;

public class RecursoProjetoDTO : BaseDTO
{
    public Guid IdRecurso { get; set; }

    public Guid IdProjeto { get; set; }

    public RecursoDTO? Recurso { get; set; }

    public ProjetoDTO? Projeto { get; set; }
}