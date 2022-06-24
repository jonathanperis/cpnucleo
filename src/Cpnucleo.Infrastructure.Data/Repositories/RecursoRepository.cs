using Cpnucleo.Infrastructure.Data.Context;
using Cpnucleo.Infrastructure.Data.Extensions;

namespace Cpnucleo.Infrastructure.Data.Repositories;

internal class RecursoRepository : GenericRepository<Recurso>, IRecursoRepository
{
    public RecursoRepository(CpnucleoContext context)
        : base(context) { }

    public Task<Recurso> AddAsync(Recurso entity, string salt)
    {
        entity.Salt = salt;

        return AddAsync(entity);
    }

    public async Task<Recurso> GetRecursoByLoginAsync(string login)
    {
        return await _context.Set<Recurso>()
            .AsQueryable()
            .Include(_context.GetIncludePaths(typeof(Recurso)))
            .FirstOrDefaultAsync(x => x.Login == login && x.Ativo);
    }
}
