using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cpnucleo.Application.ViewModels
{
    public class RecursoViewModel : BaseViewModel
    {
        public RecursoViewModel()
        {
            Ativo = true;
        }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Necessário informar o {0} do Recurso.")]
        [MaxLength(50, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
        public string Nome { get; set; }

        [Display(Name = "Ativo")]
        [Column("REC_ATIVO", TypeName = "bit")]
        public bool Ativo { get; set; }

        [Display(Name = "Login")]
        [Required(ErrorMessage = "Necessário informar o {0} do Recurso.")]
        [MaxLength(50, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
        public string Login { get; set; }

        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Necessário informar a {0} do Recurso.")]
        [StringLength(50, ErrorMessage = "A {0} deve conter ao menos {2} e o máximo {1} caractéres.", MinimumLength = 8)]
        public string Senha { get; set; }

        [Display(Name = "Confirmação de Senha")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Necessário informar a {0} do Recurso.")]
        [Compare("Senha", ErrorMessage = "A {1} e a {0} não correspondem.")]
        public string ConfirmarSenha { get; set; }

        public string Salt { get; set; }
    }
}