namespace Cpnucleo.Application.Commands.Tarefa.UpdateTarefa;

public class UpdateTarefaCommand : IRequest<OperationResult>
{
    public Guid Id { get; }
    public string Nome { get; }
    public DateTime DataInicio { get; }
    public DateTime DataTermino { get; }
    public int QtdHoras { get; }
    public string Detalhe { get; }
    public bool Ativo { get; }
    public Guid IdProjeto { get; }
    public Guid IdWorkflow { get; }
    public Guid IdRecurso { get; }
    public Guid IdTipoTarefa { get; }

    public UpdateTarefaCommand(Guid id, string nome, DateTime dataInicio, DateTime dataTermino, int qtdHoras, string detalhe, bool ativo, Guid idProjeto, Guid idWorkflow, Guid idRecurso, Guid idTipoTarefa)
    {
        Id = id;
        Nome = nome;
        DataInicio = dataInicio;
        DataTermino = dataTermino;
        QtdHoras = qtdHoras;
        Detalhe = detalhe;
        Ativo = ativo;
        IdProjeto = idProjeto;
        IdWorkflow = idWorkflow;
        IdRecurso = idRecurso;
        IdTipoTarefa = idTipoTarefa;
    }
}
