using Cpnucleo.Infrastructure.Context;
using Cpnucleo.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Cpnucleo.Infrastructure.Repositories;

internal class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly CpnucleoDbContext _context;

    public GenericRepository(CpnucleoDbContext context)
    {
        _context = context;
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        if (entity.Id == Guid.Empty)
        {
            entity.Id = Guid.NewGuid();
        }

        entity.Ativo = true;
        entity.DataInclusao = DateTime.UtcNow;

        EntityEntry<TEntity> result = await _context.AddAsync(entity);

        return result.Entity;
    }

    public void Update(TEntity entity)
    {
        entity.DataAlteracao = DateTime.UtcNow;

        _context.Update(entity);
    }

    public IQueryable<TEntity> Get(Guid id)
    {
        return _context.Set<TEntity>()
            .AsQueryable()
            .Include(_context.GetIncludePaths(typeof(TEntity)))
            .Where(x => x.Id == id && x.Ativo);
    }

    protected IQueryable<TEntity> All(Expression<Func<TEntity, bool>> predicate, bool getDependencies = false)
    {
        IQueryable<TEntity> obj = _context.Set<TEntity>();

        if (getDependencies)
        {
            obj = obj.Include(_context.GetIncludePaths(typeof(TEntity)));
        }

        return obj.OrderBy(x => x.DataInclusao)
            .Where(predicate);
    }

    public IQueryable<TEntity> List(bool getDependencies = false)
    {
        Expression<Func<TEntity, bool>> predicate = x => x.Ativo;

        return All(predicate, getDependencies);
    }

    public async Task RemoveAsync(Guid id)
    {
        TEntity entity = await Get(id).FirstAsync();

        entity.Ativo = false;
        entity.DataExclusao = DateTime.UtcNow;

        Update(entity);
    }

    public void Detatch(TEntity entity)
    {
        _context.Entry(entity).State = EntityState.Detached;
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
