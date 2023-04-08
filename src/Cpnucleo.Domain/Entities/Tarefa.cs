namespace Cpnucleo.Domain.Entities;

public sealed class Tarefa : BaseEntity
{
    public string? Nome { get; private set; }

    public DateTime DataInicio { get; private set; }

    public DateTime DataTermino { get; private set; }

    public int QtdHoras { get; private set; }

    public string? Detalhe { get; private set; }

    public Guid IdProjeto { get; private set; }

    public Guid IdWorkflow { get; private set; }

    public Guid IdRecurso { get; private set; }

    public Guid IdTipoTarefa { get; private set; }

    public Projeto? Projeto { get; private set; }

    public Workflow? Workflow { get; private set; }

    public Recurso? Recurso { get; private set; }

    public TipoTarefa? TipoTarefa { get; private set; }

    public static Tarefa Create(string nome, 
                                DateTime dataInicio, 
                                DateTime dataTermino, 
                                int qtdHoras, 
                                string detalhe, 
                                Guid idProjeto, 
                                Guid idWorkflow, 
                                Guid idRecurso, 
                                Guid idTipoTarefa)
    {
        return new Tarefa
        {
            Id = Guid.NewGuid(),
            Nome = nome,
            DataInicio = dataInicio,
            DataTermino = dataTermino,
            QtdHoras = qtdHoras,
            Detalhe = detalhe,
            IdProjeto = idProjeto,
            IdWorkflow = idWorkflow,
            IdRecurso = idRecurso,
            IdTipoTarefa = idTipoTarefa,
            DataInclusao = DateTime.UtcNow,
            Ativo = true
        };
    }

    public static Tarefa Create(Tarefa item,
                                string nome,
                                DateTime dataInicio,
                                DateTime dataTermino,
                                int qtdHoras,
                                string detalhe,
                                Guid idProjeto,
                                Guid idWorkflow,
                                Guid idRecurso,
                                Guid idTipoTarefa)
    {
        item.Nome = nome;
        item.DataInicio = dataInicio;
        item.DataTermino = dataTermino;
        item.QtdHoras = qtdHoras;
        item.Detalhe = detalhe;
        item.IdProjeto = idProjeto;
        item.IdWorkflow = idWorkflow;
        item.IdRecurso = idRecurso;
        item.IdTipoTarefa = idTipoTarefa;
        item.DataAlteracao = DateTime.UtcNow;

        return item;
    }

    public static Tarefa Remove(Tarefa item)
    {
        item.Ativo = false;
        item.DataExclusao = DateTime.UtcNow;

        return item;
    }
}
