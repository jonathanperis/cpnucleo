namespace Cpnucleo.Infra.CrossCutting.Shared.Common.DTOs;

public class ProjetoDTO : BaseDTO
{
    [Display(Name = "Nome")]
    [Required(ErrorMessage = "Necess�rio informar o {0} do Projeto.")]
    [MaxLength(50, ErrorMessage = "{0} pode conter no m�ximo {1} caract�res.")]
    public string Nome { get; set; }

    [Required]
    [Display(Name = "Sistema")]
    public Guid IdSistema { get; set; }

    public SistemaDTO? Sistema { get; set; }
}