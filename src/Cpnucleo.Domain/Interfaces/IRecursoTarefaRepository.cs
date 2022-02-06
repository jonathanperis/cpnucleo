namespace Cpnucleo.Domain.Interfaces;

public interface IRecursoTarefaRepository : IGenericRepository<RecursoTarefa>
{
    Task<IEnumerable<RecursoTarefa>> GetRecursoTarefaByTarefaAsync(Guid idTarefa);
}
