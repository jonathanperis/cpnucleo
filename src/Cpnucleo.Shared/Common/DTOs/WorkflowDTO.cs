namespace Cpnucleo.Shared.Common.DTOs;

public sealed record WorkflowDTO : BaseDTO
{
    [Display(Name = "Nome")]
    [Required(ErrorMessage = "Necess�rio informar o {0} do Workflow.")]
    [MaxLength(50, ErrorMessage = "{0} pode conter no m�ximo {1} caract�res.")]
    public string Nome { get; set; }

    [Display(Name = "Ordem")]
    [Required(ErrorMessage = "Necess�rio informar a {0} do Workflow.")]
    public int Ordem { get; set; }

    [Display(Name = "Tamanho Coluna")]
    public string? TamanhoColuna { get; set; }
}