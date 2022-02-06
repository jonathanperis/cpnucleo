namespace Cpnucleo.Application.DTOs;

public class WorkflowDTO : BaseDTO
{
    public string Nome { get; set; }

    public int? Ordem { get; set; }

    public string? TamanhoColuna { get; set; }
}