namespace Cpnucleo.Infra.CrossCutting.Shared.Common.DTOs;

public class TipoTarefaDTO : BaseDTO
{
    [Display(Name = "Nome")]
    [Required(ErrorMessage = "Necess�rio informar o {0} do Tipo Tarefa.")]
    [MaxLength(50, ErrorMessage = "{0} pode conter no m�ximo {1} caract�res.")]
    public string Nome { get; set; }

    [Display(Name = "Element Card")]
    [Required(ErrorMessage = "Necess�rio informar o {0} do Tipo Tarefa.")]
    [MaxLength(100, ErrorMessage = "{0} pode conter no m�ximo {1} caract�res.")]
    public string? Element { get; set; }

    [Display(Name = "Image Card")]
    [Required(ErrorMessage = "Necess�rio informar o {0} do Tipo Tarefa.")]
    [MaxLength(100, ErrorMessage = "{0} pode conter no m�ximo {1} caract�res.")]
    public string Image { get; set; }
}