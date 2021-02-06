using System;
using System.ComponentModel.DataAnnotations;

namespace Cpnucleo.RazorPages.Models
{
    public class ApontamentoViewModel : BaseViewModel
    {
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Necessário informar a {0} do Apontamento.")]
        [MaxLength(450, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
        public string Descricao { get; set; }

        [Display(Name = "Data de Apontamento")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date, ErrorMessage = "{0} em formato inválido")]
        [Required(ErrorMessage = "Necessário informar a {0}.")]
        public DateTime? DataApontamento { get; set; }

        [Display(Name = "Tempo Utilizado")]
        [Required(ErrorMessage = "Necessário informar o {0}.")]
        [Range(1, 24, ErrorMessage = "{0} deve estar entre {1} e {2}.")]
        public int QtdHoras { get; set; }

        [Required]
        [Display(Name = "Tarefa")]
        public Guid IdTarefa { get; set; }

        [Required]
        [Display(Name = "Recurso")]
        public Guid IdRecurso { get; set; }

        public TarefaViewModel Tarefa { get; set; }
    }
}