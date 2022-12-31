using Cpnucleo.Infrastructure.Context;
using Cpnucleo.Infrastructure.Extensions;

namespace Cpnucleo.Infrastructure.Repositories;

internal sealed class RecursoRepository : GenericRepository<Recurso>, IRecursoRepository
{
    public RecursoRepository(CpnucleoDbContext context)
        : base(context) { }

    public Task<Recurso> AddAsync(Recurso entity, string salt)
    {
        entity.Salt = salt;

        return AddAsync(entity);
    }

    public IQueryable<Recurso> GetRecursoByLogin(string login)
    {
        return _context.Set<Recurso>()
            .AsQueryable()
            .Include(_context.GetIncludePaths(typeof(Recurso)))
            .Where(x => x.Login == login && x.Ativo);
    }
}
