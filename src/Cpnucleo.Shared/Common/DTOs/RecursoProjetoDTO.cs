namespace Cpnucleo.Shared.Common.Dtos;

public sealed record RecursoProjetoDto : BaseDto
{
    [Display(Name = "Recurso")]
    [Required(ErrorMessage = "Necess�rio informar o {0}.")]
    public Guid IdRecurso { get; set; }

    [Display(Name = "Projeto")]
    [Required(ErrorMessage = "Necess�rio informar o {0}.")]
    public Guid IdProjeto { get; set; }

    public RecursoDto? Recurso { get; set; }

    public ProjetoDto? Projeto { get; set; }
}