namespace Cpnucleo.Application.Common.Repositories.Interfaces;

public interface ITarefaRepository : IGenericRepository<Tarefa>
{
    Task<IEnumerable<Tarefa>> GetTarefaByRecursoAsync(Guid idRecurso);
}
