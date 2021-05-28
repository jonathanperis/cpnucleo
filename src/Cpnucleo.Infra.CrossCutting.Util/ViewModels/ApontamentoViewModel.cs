using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.ViewModels
{
    [DataContract]
    public class ApontamentoViewModel
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
        [Required(ErrorMessage = "Necessário informar a {0} do Apontamento.")]
        [MaxLength(450, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
        [DataMember(Order = 3)]
        public string Descricao { get; set; }

        [Display(Name = "Data de Apontamento")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date, ErrorMessage = "{0} em formato inválido")]
        [Required(ErrorMessage = "Necessário informar a {0}.")]
        [DataMember(Order = 4)]
        public DateTime? DataApontamento { get; set; }

        [Display(Name = "Tempo Utilizado")]
        [Required(ErrorMessage = "Necessário informar o {0}.")]
        [Range(1, 24, ErrorMessage = "{0} deve estar entre {1} e {2}.")]
        [DataMember(Order = 5)]
        public int QtdHoras { get; set; }

        [Required]
        [Display(Name = "Tarefa")]
        [DataMember(Order = 6)]
        public Guid IdTarefa { get; set; }

        [Required]
        [Display(Name = "Recurso")]
        [DataMember(Order = 7)]
        public Guid IdRecurso { get; set; }

        [DataMember(Order = 8)]
        public TarefaViewModel Tarefa { get; set; }
    }
}