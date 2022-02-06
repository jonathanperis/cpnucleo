namespace Cpnucleo.Domain.Interfaces;

public interface ITarefaRepository : IGenericRepository<Tarefa>
{
    Task<IEnumerable<Tarefa>> GetTarefaByRecursoAsync(Guid idRecurso);
}
