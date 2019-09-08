using System;
using System.ComponentModel.DataAnnotations;

namespace Cpnucleo.Application.ViewModels
{
    public class RecursoProjetoViewModel : BaseViewModel
    {
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