namespace Cpnucleo.Shared.Common.DTOs;

public sealed record RecursoProjetoDTO : BaseDTO
{
    [Display(Name = "Recurso")]
    [Required(ErrorMessage = "Necessário informar o {0}.")]
    public Guid IdRecurso { get; set; }

    [Display(Name = "Projeto")]
    [Required(ErrorMessage = "Necessário informar o {0}.")]
    public Guid IdProjeto { get; set; }

    public RecursoDTO? Recurso { get; set; }

    public ProjetoDTO? Projeto { get; set; }
}