using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cpnucleo.Application.ViewModels
{
    public class TarefaViewModel : BaseViewModel //:IValidatableObject
    {
        [Display(Name = "Nome")]  
        [Required(ErrorMessage = "Necessário informar o {0} da Tarefa.")]
        [MaxLength(450, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]    
        public string Nome { get; set; }

        [Display(Name = "Data de Início")]      
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]        
        [Required(ErrorMessage = "Necessário informar a {0} da Tarefa.")]
        public DateTime? DataInicio { get; set; }

        [Display(Name = "Data de Término")]      
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Necessário informar a {0} da Tarefa.")]
        public DateTime? DataTermino { get; set; }

        [Display(Name = "Tempo Estimado")]      
        [Required(ErrorMessage = "Necessário informar o {0} da Tarefa.")]
        public int QtdHoras { get; set; }        

        [Display(Name = "Detalhe")]     
        [MaxLength(1000, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")] 
        public string Detalhe { get; set; }        

        [Display(Name = "Percentual Concluído")]      
        public int? PercentualConcluido { get; set; }         

        [Display(Name = "Projeto")]      
        public Guid IdProjeto { get; set; }

        [Display(Name = "Workflow")]      
        public Guid? IdWorkflow { get; set; } 

        [Display(Name = "Recurso")]      
        public Guid? IdRecurso { get; set; }         

        [Display(Name = "Tipo Tarefa")]      
        public Guid? IdTipoTarefa { get; set; }    

        public ProjetoViewModel Projeto { get; set; }

        public WorkflowViewModel Workflow { get; set; }

        public RecursoViewModel Recurso { get; set; }

        public TipoTarefaViewModel TipoTarefa { get; set; }

        public List<ImpedimentoTarefaViewModel> ListaImpedimentos { get; set; }

        public List<ApontamentoViewModel> ListaApontamentos { get; set; }

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