using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.ViewModels
{
    [DataContract]
    public class ImpedimentoTarefaViewModel
    {
        [Key]
        [Display(Name = "Id")]
        [DataMember(Order = 1)]
        public Guid Id { get; set; }

        [Display(Name = "Data de Inclusão")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataMember(Order = 2)]
        public DateTime DataInclusao { get; set; }

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Necessário informar a {0} do Impedimento.")]
        [MaxLength(450, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
        [DataMember(Order = 3)]
        public string Descricao { get; set; }

        [Required]
        [Display(Name = "Tarefa")]
        [DataMember(Order = 4)]
        public Guid IdTarefa { get; set; }

        [Required]
        [Display(Name = "Impedimento")]
        [DataMember(Order = 5)]
        public Guid IdImpedimento { get; set; }

        [DataMember(Order = 6)]
        public TarefaViewModel Tarefa { get; set; }

        [DataMember(Order = 7)]
        public ImpedimentoViewModel Impedimento { get; set; }
    }
}