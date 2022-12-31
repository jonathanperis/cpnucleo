namespace Cpnucleo.Domain.Common.Repositories.Interfaces;

public interface IRecursoTarefaRepository : IGenericRepository<RecursoTarefa>
{
    IQueryable<RecursoTarefa> GetRecursoTarefaByTarefa(Guid idTarefa);
}
