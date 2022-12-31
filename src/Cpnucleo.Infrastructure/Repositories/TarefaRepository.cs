using Cpnucleo.Infrastructure.Context;
using Cpnucleo.Infrastructure.Extensions;

namespace Cpnucleo.Infrastructure.Repositories;

internal sealed class TarefaRepository : GenericRepository<Tarefa>, ITarefaRepository
{
    public TarefaRepository(CpnucleoDbContext context)
        : base(context) { }

    public IQueryable<Tarefa> ListTarefaByRecurso(Guid idRecurso)
    {
        return _context.Set<RecursoTarefa>()
            .AsQueryable()
            .Include(_context.GetIncludePaths(typeof(RecursoTarefa)))
            .Where(x => x.IdRecurso == idRecurso && x.Ativo)
            .Select(x => x.Tarefa);
    }
}
