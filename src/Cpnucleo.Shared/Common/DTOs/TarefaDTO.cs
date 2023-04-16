namespace Cpnucleo.Shared.Common.Dtos;

public sealed record TarefaDto : BaseDto
{
    public string? Nome { get; set; }

    public DateTime DataInicio { get; set; }

    public DateTime DataTermino { get; set; }

    public int QtdHoras { get; set; }

    public string? Detalhe { get; set; }

    public int HorasConsumidas { get; set; }

    public int HorasRestantes { get; set; }

    public Guid IdProjeto { get; set; }

    public Guid IdWorkflow { get; set; }

    public Guid IdRecurso { get; set; }

    public Guid IdTipoTarefa { get; set; }

    public ProjetoDto? Projeto { get; set; }

    public WorkflowDto? Workflow { get; set; }

    public RecursoDto? Recurso { get; set; }

    public TipoTarefaDto? TipoTarefa { get; set; }
}