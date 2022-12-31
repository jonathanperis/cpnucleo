namespace Cpnucleo.Domain.Common.Repositories.Interfaces;

public interface ITarefaRepository : IGenericRepository<Tarefa>
{
    IQueryable<Tarefa> ListTarefaByRecurso(Guid idRecurso);
}
