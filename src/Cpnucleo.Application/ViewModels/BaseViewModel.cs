using System;
using System.ComponentModel.DataAnnotations;

namespace Cpnucleo.Application.ViewModels
{
    public abstract class BaseViewModel
    {
        [Key]
        [Display(Name = "Código")]
        public Guid Id { get; set; }

        [Display(Name = "Data de Inclusão")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DataInclusao { get; set; }

        [Display(Name = "Data de Alteração")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DataAlteracao { get; set; }
    }
}
