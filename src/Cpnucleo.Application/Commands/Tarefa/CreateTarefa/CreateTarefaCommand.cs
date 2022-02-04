namespace Cpnucleo.Application.Commands.Tarefa.CreateTarefa;

public class CreateTarefaCommand : IRequest<OperationResult>
{
    public Guid Id { get; }
    public string Nome { get; }
    public DateTime DataInicio { get; }
    public DateTime DataTermino { get; }
    public int QtdHoras { get; }
    public string Detalhe { get; }
    public Guid IdProjeto { get; }
    public Guid IdWorkflow { get; }
    public Guid IdRecurso { get; }
    public Guid IdTipoTarefa { get; }

    public CreateTarefaCommand(Guid id, string nome, DateTime dataInicio, DateTime dataTermino, int qtdHoras, string detalhe, Guid idProjeto, Guid idWorkflow, Guid idRecurso, Guid idTipoTarefa)
    {
        Id = id;
        Nome = nome;
        DataInicio = dataInicio;
        DataTermino = dataTermino;
        QtdHoras = qtdHoras;
        Detalhe = detalhe;
        IdProjeto = idProjeto;
        IdWorkflow = idWorkflow;
        IdRecurso = idRecurso;
        IdTipoTarefa = idTipoTarefa;
    }
}
