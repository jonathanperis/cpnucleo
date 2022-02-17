namespace Cpnucleo.Infra.CrossCutting.Util.Common.DTOs;

public abstract class BaseDTO
{
    public Guid Id { get; set; }

    public DateTime DataInclusao { get; set; }
}