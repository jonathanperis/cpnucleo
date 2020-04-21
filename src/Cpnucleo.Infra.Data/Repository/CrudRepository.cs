using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Repositories;
using Cpnucleo.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Cpnucleo.Infra.Data.Repository
{
    internal class CrudRepository<TEntity> : ICrudRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly CpnucleoContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public CrudRepository(CpnucleoContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public void Incluir(TEntity obj)
        {
            _dbSet.Add(obj);
        }

        public TEntity Consultar(Guid id)
        {
            return _dbSet
                .AsNoTracking()
                .Include(_context.GetIncludePaths(typeof(TEntity)))
                .FirstOrDefault(x => x.Id == id && x.Ativo);
        }

        public IQueryable<TEntity> Listar(bool getDependencies = false)
        {
            IQueryable<TEntity> obj = _dbSet.AsNoTracking();

            if (getDependencies)
            {
                obj = obj.Include(_context.GetIncludePaths(typeof(TEntity)));
            }

            return obj.OrderBy(x => x.DataInclusao)
                .Where(x => x.Ativo);
        }

        public void Alterar(TEntity obj)
        {
            _dbSet.Update(obj);
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
