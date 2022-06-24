using Cpnucleo.Infrastructure.Data.Context;

namespace Cpnucleo.Infrastructure.Data.Repositories;

internal class RecursoTarefaRepository : GenericRepository<RecursoTarefa>, IRecursoTarefaRepository
{
    public RecursoTarefaRepository(CpnucleoContext context)
        : base(context) { }

    public async Task<IEnumerable<RecursoTarefa>> GetRecursoTarefaByTarefaAsync(Guid idTarefa)
    {
        Expression<Func<RecursoTarefa, bool>> predicate = x => x.IdTarefa == idTarefa && x.Ativo;

        return await All(predicate, true)
            .ToListAsync();
    }
}
