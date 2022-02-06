namespace Cpnucleo.Application.Common.DTOs;

public class WorkflowDTO : BaseDTO
{
    public string Nome { get; set; }

    public int? Ordem { get; set; }

    public string? TamanhoColuna { get; set; }
}