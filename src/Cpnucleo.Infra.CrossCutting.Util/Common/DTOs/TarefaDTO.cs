namespace Cpnucleo.Infra.CrossCutting.Util.Common.DTOs;

public class TarefaDTO : BaseDTO //:IValidatableObject
{
    [Display(Name = "Nome")]
    [Required(ErrorMessage = "Necess�rio informar o {0} da Tarefa.")]
    [MaxLength(450, ErrorMessage = "{0} pode conter no m�ximo {1} caract�res.")]
    public string Nome { get; set; }

    [Display(Name = "Data de In�cio")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    [DataType(DataType.Date, ErrorMessage = "{0} em formato inv�lido")]
    [Required(ErrorMessage = "Necess�rio informar a {0} da Tarefa.")]
    public DateTime? DataInicio { get; set; }

    [Display(Name = "Data de T�rmino")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    [DataType(DataType.Date, ErrorMessage = "{0} em formato inv�lido")]
    [Required(ErrorMessage = "Necess�rio informar a {0} da Tarefa.")]
    public DateTime? DataTermino { get; set; }

    [Display(Name = "Tempo Estimado")]
    [Required(ErrorMessage = "Necess�rio informar o {0} da Tarefa.")]
    public int QtdHoras { get; set; }

    [Display(Name = "Detalhe")]
    [MaxLength(1000, ErrorMessage = "{0} pode conter no m�ximo {1} caract�res.")]
    public string Detalhe { get; set; }

    public int HorasConsumidas { get; set; }

    public int HorasRestantes { get; set; }

    [Display(Name = "Projeto")]
    [Required(ErrorMessage = "Necess�rio informar o {0} da Tarefa.")]
    public Guid IdProjeto { get; set; }

    [Display(Name = "Workflow")]
    [Required(ErrorMessage = "Necess�rio informar o {0} da Tarefa.")]
    public Guid IdWorkflow { get; set; }

    [Display(Name = "Recurso")]
    [Required(ErrorMessage = "Necess�rio informar o {0} da Tarefa.")]
    public Guid IdRecurso { get; set; }

    [Display(Name = "Tipo Tarefa")]
    [Required(ErrorMessage = "Necess�rio informar o {0} da Tarefa.")]
    public Guid IdTipoTarefa { get; set; }

    public ProjetoDTO? Projeto { get; set; }

    public WorkflowDTO? Workflow { get; set; }

    public RecursoDTO? Recurso { get; set; }

    public TipoTarefaDTO? TipoTarefa { get; set; }

    // public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    // {
    //     if (DataInicio < DateTime.Now.Date)
    //     {
    //         yield return new ValidationResult(
    //             "Data de In�cio n�o pode ser anterior a data atual.",
    //             new[] { "DataInicio" });                
    //     }

    //     if (DataTermino < DataInicio)
    //     {
    //         yield return new ValidationResult(
    //             "Data de T�rmino n�o pode ser anterior a Data de In�cio.",
    //             new[] { "DataTermino" });                
    //     }
    // }
}