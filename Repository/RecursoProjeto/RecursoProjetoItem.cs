using dotnet_cpnucleo_pages.Repository.Projeto;
using dotnet_cpnucleo_pages.Repository.Recurso;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_cpnucleo_pages.Repository.RecursoProjeto
{
    [Table("CPN_TB_RECURSO_PROJETO")]
    public class RecursoProjetoItem
    {
        [Key]
        [Display(Name = "Código Recurso Projeto")]      
        [Column("RPROJ_ID", TypeName = "int")]
        public int IdRecursoProjeto { get; set; }

        [Display(Name = "Data de Inclusão")]      
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column("RPROJ_DATA_INCLUSAO", TypeName = "datetime")]
        public DateTime? DataInclusao { get; set; }     

        [Display(Name = "Recurso")]      
        [Required(ErrorMessage = "Necessário informar o {0}.")]
        [Column("REC_ID", TypeName = "int")]
        public int IdRecurso { get; set; }

        [Display(Name = "Projeto")]      
        [Column("PROJ_ID", TypeName = "int")]
        public int IdProjeto { get; set; }

        public RecursoItem Recurso { get; set; }

        public ProjetoItem Projeto { get; set; }
    }
}