using dotnet_cpnucleo_pages.Repository.Sistema;
using System;
using System.ComponentModel.DataAnnotations;

namespace dotnet_cpnucleo_pages.Repository2.Projeto
{
    public class ProjetoItem
    {
        [Key]
        [Display(Name = "Código Projeto")]      
        public int IdProjeto { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Necessário informar o {0} do Projeto.")]
        [MaxLength(50, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
        public string Nome { get; set; }        

        [Display(Name = "Data de Inclusão")]      
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DataInclusao { get; set; }

        [Display(Name = "Data de Alteração")]      
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DataAlteracao { get; set; }                

        [Required]
        [Display(Name = "Sistema")]      
        public int IdSistema { get; set; }

        public SistemaItem Sistema { get; set; }
    }
}