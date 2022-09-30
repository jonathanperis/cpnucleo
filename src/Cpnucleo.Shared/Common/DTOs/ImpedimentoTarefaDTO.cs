namespace Cpnucleo.Shared.Common.DTOs;

public sealed record ImpedimentoTarefaDTO : BaseDTO
{
    [Display(Name = "Descrição")]
    [Required(ErrorMessage = "Necessário informar a {0} do Impedimento.")]
    [MaxLength(450, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
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