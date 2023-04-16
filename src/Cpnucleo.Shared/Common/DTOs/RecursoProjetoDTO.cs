namespace Cpnucleo.Shared.Common.Dtos;

public sealed record RecursoProjetoDto : BaseDto
{
    public Guid IdRecurso { get; set; }

    public Guid IdProjeto { get; set; }

    public RecursoDto? Recurso { get; set; }

    public ProjetoDto? Projeto { get; set; }
}