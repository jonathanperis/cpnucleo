namespace Cpnucleo.Infra.Data.Repositories;

internal class RecursoRepository : GenericRepository<Recurso>, IRecursoRepository
{
    public RecursoRepository(CpnucleoContext context)
        : base(context)
    {

    }

    public async Task<Recurso> GetByLoginAsync(string login)
    {
        return await _context.Set<Recurso>()
            .AsQueryable()
            .Include(_context.GetIncludePaths(typeof(Recurso)))
            .FirstOrDefaultAsync(x => x.Login == login && x.Ativo);
    }
}
