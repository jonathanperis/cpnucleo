namespace Cpnucleo.Shared.Common.Dtos;

public sealed record ApontamentoDto : BaseDto
{
    [Display(Name = "Descri��o")]
    [Required(ErrorMessage = "Necess�rio informar a {0} do Apontamento.")]
    [MaxLength(450, ErrorMessage = "{0} pode conter no m�ximo {1} caract�res.")]
    public string Descricao { get; set; }

    [Display(Name = "Data de Apontamento")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
    [DataType(DataType.Date, ErrorMessage = "{0} em formato inv�lido")]
    [Required(ErrorMessage = "Necess�rio informar a {0}.")]
    public DateTime DataApontamento { get; set; }

    [Display(Name = "Tempo Utilizado")]
    [Required(ErrorMessage = "Necess�rio informar o {0}.")]
    [Range(1, 24, ErrorMessage = "{0} deve estar entre {1} e {2}.")]
    public int QtdHoras { get; set; }

    [Required]
    [Display(Name = "Tarefa")]
    public Guid IdTarefa { get; set; }

    [Required]
    [Display(Name = "Recurso")]
    public Guid IdRecurso { get; set; }

    public TarefaDto? Tarefa { get; set; }
}