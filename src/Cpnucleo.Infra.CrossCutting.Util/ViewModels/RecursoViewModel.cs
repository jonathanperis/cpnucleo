namespace Cpnucleo.Infra.CrossCutting.Util.ViewModels
{
    [DataContract]
    public class RecursoViewModel : BaseViewModel
    {
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Necessário informar o {0} do Recurso.")]
        [MaxLength(50, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
        [DataMember(Order = 3)]
        public string Nome { get; set; }

        [Display(Name = "Login")]
        [Required(ErrorMessage = "Necessário informar o {0} do Recurso.")]
        [MaxLength(50, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
        [DataMember(Order = 4)]
        public string Login { get; set; }

        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Necessário informar a {0} do Recurso.")]
        [StringLength(50, ErrorMessage = "A {0} deve conter ao menos {2} e o máximo {1} caractéres.", MinimumLength = 8)]
        [DataMember(Order = 5)]
        public string Senha { get; set; }

        [Display(Name = "Confirmação de Senha")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Necessário informar a {0} do Recurso.")]
        [Compare("Senha", ErrorMessage = "A {1} e a {0} não correspondem.")]
        [DataMember(Order = 6)]
        public string ConfirmarSenha { get; set; }

        [DataMember(Order = 7)]
        public string Salt { get; set; }

        [DataMember(Order = 8)]
        public string Token { get; set; }
    }
}