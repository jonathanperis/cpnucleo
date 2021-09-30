namespace Cpnucleo.Infra.CrossCutting.Util.ViewModels;

[DataContract]
public class ProjetoViewModel : BaseViewModel
{
    [Display(Name = "Nome")]
    [Required(ErrorMessage = "Necessário informar o {0} do Projeto.")]
    [MaxLength(50, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
    [DataMember(Order = 3)]
    public string Nome { get; set; }

    [Required]
    [Display(Name = "Sistema")]
    [DataMember(Order = 4)]
    public Guid IdSistema { get; set; }

    [DataMember(Order = 5)]
    public SistemaViewModel Sistema { get; set; }
}