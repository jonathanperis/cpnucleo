namespace Cpnucleo.Infra.CrossCutting.Util.ViewModels
{
    [DataContract]
    public class WorkflowViewModel : BaseViewModel
    {
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Necessário informar o {0} do Workflow.")]
        [MaxLength(50, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
        [DataMember(Order = 3)]
        public string Nome { get; set; }

        [Display(Name = "Ordem")]
        [Required(ErrorMessage = "Necessário informar a {0} do Workflow.")]
        [DataMember(Order = 4)]
        public int? Ordem { get; set; }

        [Display(Name = "Tamanho Coluna")]
        [DataMember(Order = 5)]
        public string TamanhoColuna { get; set; }
    }
}