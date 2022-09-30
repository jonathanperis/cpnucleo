using Cpnucleo.Infrastructure.Data.Context;

namespace Cpnucleo.Infrastructure.Data.Repositories;

internal sealed class RecursoProjetoRepository : GenericRepository<RecursoProjeto>, IRecursoProjetoRepository
{
    public RecursoProjetoRepository(CpnucleoContext context)
        : base(context) { }

    public async Task<IEnumerable<RecursoProjeto>> GetRecursoProjetoByProjetoAsync(Guid idProjeto)
    {
        Expression<Func<RecursoProjeto, bool>> predicate = x => x.IdProjeto == idProjeto && x.Ativo;

        return await All(predicate, true)
            .ToListAsync();
    }
}
