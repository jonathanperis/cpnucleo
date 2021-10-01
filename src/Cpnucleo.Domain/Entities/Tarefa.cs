namespace Cpnucleo.Domain.Entities;

public class Tarefa : BaseEntity
{
    public string Nome { get; set; }

    public DateTime DataInicio { get; set; }

    public DateTime DataTermino { get; set; }

    public int QtdHoras { get; set; }

    public string Detalhe { get; set; }

    public int HorasConsumidas { get; set; }

    public int HorasRestantes { get; set; }

    public Guid IdProjeto { get; set; }

    public Guid IdWorkflow { get; set; }

    public Guid IdRecurso { get; set; }

    public Guid IdTipoTarefa { get; set; }

    public Projeto Projeto { get; set; }

    public Workflow Workflow { get; set; }

    public Recurso Recurso { get; set; }

    public TipoTarefa TipoTarefa { get; set; }
}
