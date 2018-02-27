using dotnet_cpnucleo_pages.Repository.Impedimento;
using dotnet_cpnucleo_pages.Repository.Tarefa;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_cpnucleo_pages.Repository.ImpedimentoTarefa
{
    [Table("CPN_TB_TAREFA_IMPEDIMENTO")]
    public class ImpedimentoTarefaItem
    {
        [Key]
        [Display(Name = "Código Impedimento Tarefa")]      
        [Column("ITAR_ID", TypeName = "int")]
        public int IdImpedimentoTarefa { get; set; }

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Necessário informar a {0} do Impedimento.")]
        [MaxLength(450, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
        [Column("ITAR_DESCRICAO", TypeName = "varchar(450)")]
        public string Descricao { get; set; }        

        [Display(Name = "Ativo")]      
        [Column("ITAR_ATIVO", TypeName = "bit")]
        public bool Ativo { get; set; }

        [Display(Name = "Data de Inclusão")]      
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column("ITAR_DATA_INCLUSAO", TypeName = "datetime")]
        public DateTime? DataInclusao { get; set; }

        [Display(Name = "Data de Alteração")]      
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column("ITAR_DATA_ALTERACAO", TypeName = "datetime")]
        public DateTime? DataAlteracao { get; set; }        

        [Required]
        [Display(Name = "Tarefa")]      
        [Column("TAR_ID", TypeName = "int")]
        public int IdTarefa { get; set; }        

        [Required]
        [Display(Name = "Impedimento")]      
        [Column("IMP_ID", TypeName = "int")]
        public int IdImpedimento { get; set; }

        public TarefaItem Tarefa { get; set; }

        public ImpedimentoItem Impedimento { get; set; }        
    }
}