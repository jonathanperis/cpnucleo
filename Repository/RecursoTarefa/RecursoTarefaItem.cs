using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using dotnet_cpnucleo_pages.Repository.Tarefa;
using dotnet_cpnucleo_pages.Repository.Recurso;

namespace dotnet_cpnucleo_pages.Repository.RecursoTarefa
{
    [Table("CPN_TB_RECURSO_TAREFA")]
    public class RecursoTarefaItem
    {
        [Key]
        [Display(Name = "Código Recurso Tarefa")]      
        [Column("RTAR_ID", TypeName = "int")]
        public int IdRecursoTarefa { get; set; }

        [Display(Name = "Percentual")]    
        [Required(ErrorMessage = "Necessário informar o {0}.")]
        [Range(1, 100, ErrorMessage = "{0} deve estar entre {1} e {2}.")]  
        [Column("RTAR_PERCENTUAL", TypeName = "int")]
        public int? PercentualTarefa { get; set; }

        [Display(Name = "Data de Inclusão")]      
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column("RTAR_DATA_INCLUSAO", TypeName = "datetime")]
        public DateTime? DataInclusao { get; set; }

        [Display(Name = "Data de Alteração")]      
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column("RTAR_DATA_ALTERACAO", TypeName = "datetime")]
        public DateTime? DataAlteracao { get; set; }                

        [Display(Name = "Recurso")]      
        [Required(ErrorMessage = "Necessário informar o {0}.")]
        [Column("REC_ID", TypeName = "int")]
        public int IdRecurso { get; set; }

        public RecursoItem Recurso { get; set; }

        [Display(Name = "Tarefa")]      
        [Column("TAR_ID", TypeName = "int")]
        public int IdTarefa { get; set; }

        [NotMapped]
        public int HorasUtilizadas { get; set; }

        [NotMapped]
        public int HorasDisponiveis { get; set; }

        public TarefaItem Tarefa { get; set; }
    }
}