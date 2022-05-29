namespace Cpnucleo.Infra.CrossCutting.Util.Common.DTOs;

public abstract class BaseDTO
{
    [Key]
    [Display(Name = "Id")]
    public Guid Id { get; set; }

    [Display(Name = "Data de Inclusão")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime DataInclusao { get; set; }
}