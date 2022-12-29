using Cpnucleo.Infrastructure.Context;

namespace Cpnucleo.Infrastructure.Repositories;

internal sealed class RecursoProjetoRepository : GenericRepository<RecursoProjeto>, IRecursoProjetoRepository
{
    public RecursoProjetoRepository(CpnucleoDbContext context)
        : base(context) { }

    public async Task<IEnumerable<RecursoProjeto>> GetRecursoProjetoByProjetoAsync(Guid idProjeto)
    {
        Expression<Func<RecursoProjeto, bool>> predicate = x => x.IdProjeto == idProjeto && x.Ativo;

        return await All(predicate, true)
            .ToListAsync();
    }
}
