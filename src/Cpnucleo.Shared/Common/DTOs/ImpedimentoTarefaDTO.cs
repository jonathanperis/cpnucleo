namespace Cpnucleo.Shared.Common.DTOs;

public sealed record ImpedimentoTarefaDTO : BaseDTO
{
    [Display(Name = "Descri��o")]
    [Required(ErrorMessage = "Necess�rio informar a {0} do Impedimento.")]
    [MaxLength(450, ErrorMessage = "{0} pode conter no m�ximo {1} caract�res.")]
    public string Descricao { get; set; }

    [Required]
    [Display(Name = "Tarefa")]
    public Guid IdTarefa { get; set; }

    [Required]
    [Display(Name = "Impedimento")]
    public Guid IdImpedimento { get; set; }

    public TarefaDTO? Tarefa { get; set; }

    public ImpedimentoDTO? Impedimento { get; set; }
}