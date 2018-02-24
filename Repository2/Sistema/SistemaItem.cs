using System;
using System.ComponentModel.DataAnnotations;

namespace dotnet_cpnucleo_pages.Repository2.Sistema
{
    public class SistemaItem
    {
        [Key]
        [Display(Name = "Código Sistema")]      
        public int IdSistema { get; set; }

        [Display(Name = "Nome")]      
        [Required(ErrorMessage = "Necessário informar o {0} do Sistema.")]
        [MaxLength(50, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
        public string Nome { get; set; }

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Necessário informar a {0} do Sistema.")]
        [MaxLength(450, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
        public string Descricao { get; set; }        

        [Display(Name = "Data de Inclusão")]      
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DataInclusao { get; set; }

        [Display(Name = "Data de Alteração")]      
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DataAlteracao { get; set; }        
    }
}