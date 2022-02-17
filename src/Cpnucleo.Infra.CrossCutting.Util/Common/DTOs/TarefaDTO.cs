namespace Cpnucleo.Infra.CrossCutting.Util.Common.DTOs;

public class TarefaDTO : BaseDTO
{
    public string Nome { get; set; }

    public DateTime? DataInicio { get; set; }

    public DateTime? DataTermino { get; set; }

    public int QtdHoras { get; set; }

    public string Detalhe { get; set; }

    public int HorasConsumidas { get; set; }

    public int HorasRestantes { get; set; }

    public Guid IdProjeto { get; set; }

    public Guid IdWorkflow { get; set; }

    public Guid IdRecurso { get; set; }

    public Guid IdTipoTarefa { get; set; }

    public ProjetoDTO? Projeto { get; set; }

    public WorkflowDTO? Workflow { get; set; }

    public RecursoDTO? Recurso { get; set; }

    public TipoTarefaDTO? TipoTarefa { get; set; }
}