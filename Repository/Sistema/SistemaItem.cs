using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_cpnucleo_pages.Repository.Sistema
{
    [Table("CPN_TB_SISTEMA")]
    public class SistemaItem
    {
        [Key]
        [Display(Name = "Código Sistema")]      
        [Column("SIS_ID", TypeName = "int")]
        public int IdSistema { get; set; }

        [Display(Name = "Nome")]      
        [Required(ErrorMessage = "Necessário informar o {0} do Sistema.")]
        [MaxLength(50, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
        [Column("SIS_NOME", TypeName = "varchar(50)")]  
        public string Nome { get; set; }

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Necessário informar a {0} do Sistema.")]
        [MaxLength(450, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
        [Column("SIS_DESCRICAO", TypeName = "varchar(450)")]
        public string Descricao { get; set; }        

        [Display(Name = "Data de Inclusão")]      
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column("SIS_DATA_INCLUSAO", TypeName = "datetime")]
        public DateTime? DataInclusao { get; set; }

        [Display(Name = "Data de Alteração")]      
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column("SIS_DATA_ALTERACAO", TypeName = "datetime")]
        public DateTime? DataAlteracao { get; set; }        
    }
}