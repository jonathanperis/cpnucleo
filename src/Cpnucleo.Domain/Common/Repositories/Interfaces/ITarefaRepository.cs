namespace Cpnucleo.Domain.Common.Repositories.Interfaces;

public interface ITarefaRepository : IGenericRepository<Tarefa>
{
    IQueryable<Tarefa> GetTarefaByRecurso(Guid idRecurso);
}
