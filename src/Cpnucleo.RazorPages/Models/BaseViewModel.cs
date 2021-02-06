using System;
using System.ComponentModel.DataAnnotations;

namespace Cpnucleo.RazorPages.Models
{
    public abstract class BaseViewModel
    {
        [Key]
        [Display(Name = "Id")]
        public Guid Id { get; set; }

        [Display(Name = "Data de Inclusão")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DataInclusao { get; set; }
    }
}
