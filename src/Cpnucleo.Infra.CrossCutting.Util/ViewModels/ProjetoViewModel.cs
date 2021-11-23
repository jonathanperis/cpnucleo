namespace Cpnucleo.Infra.CrossCutting.Util.ViewModels;

public class ProjetoViewModel : BaseViewModel
{
    [Display(Name = "Nome")]
    [Required(ErrorMessage = "Necessário informar o {0} do Projeto.")]
    [MaxLength(50, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
    public string Nome { get; set; }

    [Required]
    [Display(Name = "Sistema")]
    public Guid IdSistema { get; set; }

    public SistemaViewModel? Sistema { get; set; }
}