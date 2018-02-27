using dotnet_cpnucleo_pages.Repository.Recurso;
using dotnet_cpnucleo_pages.Repository.Tarefa;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_cpnucleo_pages.Repository.Apontamento
{
    [Table("CPN_TB_LANCAMENTO")]
    public class ApontamentoItem
    {
        [Key]
        [Display(Name = "Código Apontamento")]      
        [Column("LANC_ID", TypeName = "int")]
        public int IdApontamento { get; set; }       

        [Display(Name = "Descrição")]      
        [Required(ErrorMessage = "Necessário informar a {0} do Apontamento.")]
        [MaxLength(450, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
        [Column("LANC_DESCRICAO", TypeName = "varchar(450)")]  
        public string Descricao { get; set; }

        [Display(Name = "Data de Apontamento")]      
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Necessário informar a {0}.")]     
        [Column("LANC_DATA_LANCAMENTO", TypeName = "datetime")]
        public DateTime? DataApontamento { get; set; }

        [Display(Name = "Tempo Utilizado")]      
        [Required(ErrorMessage = "Necessário informar o {0}.")]
        [Range(1, 24, ErrorMessage = "{0} deve estar entre {1} e {2}.")]  
        [Column("LANC_QTD_HORAS", TypeName = "int")]
        public int QtdHoras { get; set; }    

        [Display(Name = "Percentual")]      
        [Required(ErrorMessage = "Necessário informar o {0} do Apontamento.")]
        [Range(1, 100, ErrorMessage = "{0} deve estar entre {1} e {2}.")]  
        [Column("LANC_PERCENTUAL_CONCLUIDO", TypeName = "int")]
        public int? PercentualConcluido { get; set; } 

        [Display(Name = "Data de Inclusão")]      
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column("LANC_DATA_INCLUSAO", TypeName = "datetime")]
        public DateTime? DataInclusao { get; set; }

        [Display(Name = "Data de Alteração")]      
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column("LANC_DATA_ALTERACAO", TypeName = "datetime")]
        public DateTime? DataAlteracao { get; set; }                

        [Required]
        [Display(Name = "Tarefa")]      
        [Column("TAR_ID", TypeName = "int")]
        public int IdTarefa { get; set; }   

        [Required]
        [Display(Name = "Recurso")]      
        [Column("REC_ID", TypeName = "int")]
        public int IdRecurso { get; set; }

        [NotMapped]
        [Display(Name = "Semana")]      
        public string DiaSemana { get; set; }

        public TarefaItem Tarefa { get; set; }

        public RecursoItem Recurso { get; set; }
    }
}