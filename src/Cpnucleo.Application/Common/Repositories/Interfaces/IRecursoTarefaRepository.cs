namespace Cpnucleo.Application.Common.Repositories.Interfaces;

public interface IRecursoTarefaRepository : IGenericRepository<RecursoTarefa>
{
    Task<IEnumerable<RecursoTarefa>> GetRecursoTarefaByTarefaAsync(Guid idTarefa);
}
