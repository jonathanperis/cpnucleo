using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.ViewModels
{
    [DataContract]
    public class TipoTarefaViewModel : BaseViewModel
    {
        [Key]
        [Display(Name = "Id")]
        [DataMember(Order = 1)]
        public Guid Id { get; set; }

        [Display(Name = "Data de Inclusão")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataMember(Order = 2)]
        public DateTime DataInclusao { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Necessário informar o {0} do Tipo Tarefa.")]
        [MaxLength(50, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
        public string Nome { get; set; }

        [Display(Name = "Element Card")]
        [Required(ErrorMessage = "Necessário informar o {0} do Tipo Tarefa.")]
        [MaxLength(100, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
        public string Element { get; set; }

        [Display(Name = "Image Card")]
        [Required(ErrorMessage = "Necessário informar o {0} do Tipo Tarefa.")]
        [MaxLength(100, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
        public string Image { get; set; }
    }
}