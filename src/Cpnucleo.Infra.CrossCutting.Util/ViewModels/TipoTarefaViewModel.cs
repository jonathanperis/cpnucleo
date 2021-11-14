namespace Cpnucleo.Infra.CrossCutting.Util.ViewModels;

[DataContract]
public class TipoTarefaViewModel : BaseViewModel
{
    [Display(Name = "Nome")]
    [Required(ErrorMessage = "Necessário informar o {0} do Tipo Tarefa.")]
    [MaxLength(50, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
    [DataMember(Order = 3)]
    public string Nome { get; set; }

    [Display(Name = "Element Card")]
    [Required(ErrorMessage = "Necessário informar o {0} do Tipo Tarefa.")]
    [MaxLength(100, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
    [DataMember(Order = 4)]
    public string? Element { get; set; }

    [Display(Name = "Image Card")]
    [Required(ErrorMessage = "Necessário informar o {0} do Tipo Tarefa.")]
    [MaxLength(100, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
    [DataMember(Order = 5)]
    public string Image { get; set; }
}