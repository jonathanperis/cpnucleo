namespace Cpnucleo.Application.Common.DTOs;

public class ProjetoDTO : BaseDTO
{
    public string Nome { get; set; }

    public Guid IdSistema { get; set; }

    public SistemaDTO? Sistema { get; set; }
}