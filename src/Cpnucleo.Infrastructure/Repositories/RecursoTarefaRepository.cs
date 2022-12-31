using Cpnucleo.Infrastructure.Context;

namespace Cpnucleo.Infrastructure.Repositories;

internal sealed class RecursoTarefaRepository : GenericRepository<RecursoTarefa>, IRecursoTarefaRepository
{
    public RecursoTarefaRepository(CpnucleoDbContext context)
        : base(context) { }

    public IQueryable<RecursoTarefa> GetRecursoTarefaByTarefa(Guid idTarefa)
    {
        Expression<Func<RecursoTarefa, bool>> predicate = x => x.IdTarefa == idTarefa && x.Ativo;

        return All(predicate, true);
    }
}
