using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_cpnucleo_pages.Repository.TipoTarefa
{
    [Table("CPN_TB_TIPO_TAREFA")]
    public class TipoTarefaItem
    {
        [Key]
        [Display(Name = "Código Tipo Tarefa")]      
        [Column("TIP_ID", TypeName = "int")]
        public int IdTipoTarefa { get; set; }

        [Display(Name = "Nome")]      
        [Required(ErrorMessage = "Necessário informar o {0} do Tipo Tarefa.")]
        [MaxLength(50, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
        [Column("TIP_NOME", TypeName = "varchar(50)")]  
        public string Nome { get; set; }

        [Display(Name = "Data de Inclusão")]      
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column("TIP_DATA_INCLUSAO", TypeName = "datetime")]
        public DateTime? DataInclusao { get; set; }

        [Display(Name = "Data de Alteração")]      
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column("TIP_DATA_ALTERACAO", TypeName = "datetime")]
        public DateTime? DataAlteracao { get; set; }
    }
}