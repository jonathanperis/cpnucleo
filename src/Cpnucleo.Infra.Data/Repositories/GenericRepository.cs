using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.Data.Repositories
{
    internal class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly CpnucleoContext _context;

        public GenericRepository(CpnucleoContext context)
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
            entity.DataInclusao = DateTime.Now;

            EntityEntry<TEntity> result = await _context.AddAsync(entity);

            return result.Entity;
        }

        public void Update(TEntity entity)
        {
            entity.DataAlteracao = DateTime.Now;

            _context.Update(entity);
        }

        public async Task<TEntity> GetAsync(Guid id)
        {
            return await _context.Set<TEntity>()
                .AsQueryable()
                .Include(_context.GetIncludePaths(typeof(TEntity)))
                .FirstOrDefaultAsync(x => x.Id == id && x.Ativo);
        }

        public async Task<IEnumerable<TEntity>> AllAsync(bool getDependencies = false)
        {
            IQueryable<TEntity> obj = _context.Set<TEntity>();

            if (getDependencies)
            {
                obj = obj.Include(_context.GetIncludePaths(typeof(TEntity)));
            }

            return await obj.OrderBy(x => x.DataInclusao)
                .Where(x => x.Ativo)
                .ToListAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            TEntity entity = await GetAsync(id);

            entity.Ativo = false;
            entity.DataExclusao = DateTime.Now;

            Update(entity);
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}