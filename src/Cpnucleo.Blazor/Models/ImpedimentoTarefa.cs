using System;
using System.ComponentModel.DataAnnotations;

namespace Cpnucleo.Blazor.Models
{
    public class ImpedimentoTarefa : Base
    {
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Necessário informar a {0} do Impedimento.")]
        [MaxLength(450, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
        public string Descricao { get; set; }

        [Required]
        [Display(Name = "Tarefa")]
        public Guid IdTarefa { get; set; }

        [Required]
        [Display(Name = "Impedimento")]
        public Guid IdImpedimento { get; set; }

        public Tarefa Tarefa { get; set; }

        public Impedimento Impedimento { get; set; }
    }
}