using dotnet_cpnucleo_pages.Repository.Sistema;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_cpnucleo_pages.Repository.Projeto
{
    [Table("CPN_TB_PROJETO")]
    public class ProjetoItem
    {
        [Key]
        [Display(Name = "Código Projeto")]      
        [Column("PROJ_ID", TypeName = "int")]
        public int IdProjeto { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Necessário informar o {0} do Projeto.")]
        [MaxLength(50, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
        [Column("PROJ_NOME", TypeName = "varchar(80)")]
        public string Nome { get; set; }        

        [Display(Name = "Data de Inclusão")]      
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column("PROJ_DATA_INCLUSAO", TypeName = "datetime")]
        public DateTime? DataInclusao { get; set; }

        [Display(Name = "Data de Alteração")]      
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column("PROJ_DATA_ALTERACAO", TypeName = "datetime")]
        public DateTime? DataAlteracao { get; set; }                

        [Required]
        [Display(Name = "Sistema")]      
        [Column("SIS_ID", TypeName = "int")]
        public int IdSistema { get; set; }

        public SistemaItem Sistema { get; set; }
    }
}