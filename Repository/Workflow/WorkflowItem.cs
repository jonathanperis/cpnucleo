using dotnet_cpnucleo_pages.Repository.Tarefa;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_cpnucleo_pages.Repository.Workflow
{
    [Table("CPN_TB_WORKFLOW")]
    public class WorkflowItem
    {
        [Key]
        [Display(Name = "Código Workflow")]      
        [Column("WOR_ID", TypeName = "int")]
        public int IdWorkflow { get; set; }

        [Display(Name = "Nome")]      
        [Required(ErrorMessage = "Necessário informar o {0} do Workflow.")]
        [MaxLength(50, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
        [Column("WOR_NOME", TypeName = "varchar(50)")]          
        public string Nome { get; set; }

        [Display(Name = "Ordem")]      
        [Required(ErrorMessage = "Necessário informar a {0} do Workflow.")]        
        [Column("WOR_ORDEM", TypeName = "int")]
        public int? Ordem { get; set; }

        [Display(Name = "Data de Inclusão")]      
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column("WOR_DATA_INCLUSAO", TypeName = "datetime")]
        public DateTime? DataInclusao { get; set; }

        [Display(Name = "Data de Alteração")]      
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column("WOR_DATA_ALTERACAO", TypeName = "datetime")]
        public DateTime? DataAlteracao { get; set; }                

        public List<TarefaItem> ListaTarefas { get; set; }
    }
}