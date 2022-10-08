using Cpnucleo.Infrastructure.Context;
using Cpnucleo.Infrastructure.Extensions;

namespace Cpnucleo.Infrastructure.Repositories;

internal sealed class TarefaRepository : GenericRepository<Tarefa>, ITarefaRepository
{
    public TarefaRepository(CpnucleoContext context)
        : base(context) { }

    public async Task<IEnumerable<Tarefa>> GetTarefaByRecursoAsync(Guid idRecurso)
    {
        return await _context.Set<RecursoTarefa>()
            .AsQueryable()
            .Include(_context.GetIncludePaths(typeof(RecursoTarefa)))
            .Where(x => x.IdRecurso == idRecurso && x.Ativo)
            .Select(x => x.Tarefa)
            .ToListAsync();
    }
}
