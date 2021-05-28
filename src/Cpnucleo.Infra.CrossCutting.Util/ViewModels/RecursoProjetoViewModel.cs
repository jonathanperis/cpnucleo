using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.ViewModels
{
    [DataContract]
    public class RecursoProjetoViewModel : BaseViewModel
    {
        [Key]
        [Display(Name = "Id")]
        [DataMember(Order = 1)]
        public Guid Id { get; set; }

        [Display(Name = "Data de Inclusão")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataMember(Order = 2)]
        public DateTime DataInclusao { get; set; }

        [Display(Name = "Recurso")]
        [Required(ErrorMessage = "Necessário informar o {0}.")]
        public Guid IdRecurso { get; set; }

        [Display(Name = "Projeto")]
        [Required(ErrorMessage = "Necessário informar o {0}.")]
        public Guid IdProjeto { get; set; }

        public RecursoViewModel Recurso { get; set; }

        public ProjetoViewModel Projeto { get; set; }
    }
}