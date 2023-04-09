namespace Cpnucleo.Shared.Common.DTOs;

[MessagePackObject(true)]
public abstract record BaseDTO
{
    [System.ComponentModel.DataAnnotations.Key]
    [Display(Name = "Id")]
    public Guid Id { get; set; }

    [Display(Name = "Data de Inclusão")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime DataInclusao { get; set; }
}