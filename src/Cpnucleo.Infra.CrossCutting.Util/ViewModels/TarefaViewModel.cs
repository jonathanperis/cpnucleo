using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.ViewModels
{
    [DataContract]
    public class TarefaViewModel : BaseViewModel //:IValidatableObject
    {
        [Key]
        [Display(Name = "Id")]
        [DataMember(Order = 1)]
        public Guid Id { get; set; }

        [Display(Name = "Data de Inclusão")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataMember(Order = 2)]
        public DateTime DataInclusao { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Necessário informar o {0} da Tarefa.")]
        [MaxLength(450, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
        public string Nome { get; set; }

        [Display(Name = "Data de Início")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date, ErrorMessage = "{0} em formato inválido")]
        [Required(ErrorMessage = "Necessário informar a {0} da Tarefa.")]
        public DateTime? DataInicio { get; set; }

        [Display(Name = "Data de Término")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date, ErrorMessage = "{0} em formato inválido")]
        [Required(ErrorMessage = "Necessário informar a {0} da Tarefa.")]
        public DateTime? DataTermino { get; set; }

        [Display(Name = "Tempo Estimado")]
        [Required(ErrorMessage = "Necessário informar o {0} da Tarefa.")]
        public int QtdHoras { get; set; }

        [Display(Name = "Detalhe")]
        [MaxLength(1000, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
        public string Detalhe { get; set; }

        public int HorasConsumidas { get; set; }

        public int HorasRestantes { get; set; }

        [Display(Name = "Projeto")]
        [Required(ErrorMessage = "Necessário informar o {0} da Tarefa.")]
        public Guid IdProjeto { get; set; }

        [Display(Name = "Workflow")]
        [Required(ErrorMessage = "Necessário informar o {0} da Tarefa.")]
        public Guid IdWorkflow { get; set; }

        [Display(Name = "Recurso")]
        [Required(ErrorMessage = "Necessário informar o {0} da Tarefa.")]
        public Guid IdRecurso { get; set; }

        [Display(Name = "Tipo Tarefa")]
        [Required(ErrorMessage = "Necessário informar o {0} da Tarefa.")]
        public Guid IdTipoTarefa { get; set; }

        public ProjetoViewModel Projeto { get; set; }

        public WorkflowViewModel Workflow { get; set; }

        public RecursoViewModel Recurso { get; set; }

        public TipoTarefaViewModel TipoTarefa { get; set; }

        // public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        // {
        //     if (DataInicio < DateTime.Now.Date)
        //     {
        //         yield return new ValidationResult(
        //             "Data de Início não pode ser anterior a data atual.",
        //             new[] { "DataInicio" });                
        //     }

        //     if (DataTermino < DataInicio)
        //     {
        //         yield return new ValidationResult(
        //             "Data de Término não pode ser anterior a Data de Início.",
        //             new[] { "DataTermino" });                
        //     }
        // }
    }
}