using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cpnucleo.Pages.Models
{
    [Table("CPN_TB_IMPEDIMENTO")]
    public class ImpedimentoModel
    {
        [Key]
        [Display(Name = "Código Impedimento")]      
        [Column("IMP_ID", TypeName = "int")]
        public int IdImpedimento { get; set; }

        [Display(Name = "Nome")]      
        [Required(ErrorMessage = "Necessário informar o {0} do Impedimento.")]
        [MaxLength(50, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
        [Column("IMP_NOME", TypeName = "varchar(50)")]  
        public string Nome { get; set; }

        [Display(Name = "Data de Inclusão")]      
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column("IMP_DATA_INCLUSAO", TypeName = "datetime")]
        public DateTime? DataInclusao { get; set; }

        [Display(Name = "Data de Alteração")]      
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column("IMP_DATA_ALTERACAO", TypeName = "datetime")]
        public DateTime? DataAlteracao { get; set; }
    }
}