using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Repositories;
using Cpnucleo.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Cpnucleo.Infra.Data.Repository
{
    public class CrudRepository<TModel> : ICrudRepository<TModel> where TModel : BaseModel
    {
        private readonly CpnucleoContext _context;
        protected readonly DbSet<TModel> _dbSet;

        public CrudRepository(CpnucleoContext context)
        {
            _context = context;
            _dbSet = _context.Set<TModel>();
        }

        public void Incluir(TModel obj)
        {
            if (obj.Id == Guid.Empty)
            {
                obj.Id = Guid.NewGuid();
            }

            obj.Ativo = true;
            obj.DataInclusao = DateTime.Now;

            _dbSet.Add(obj);
        }

        public IQueryable<TModel> Consultar(Guid id)
        {
            return _dbSet
                .AsNoTracking()
                .Where(x => x.Id == id && x.Ativo);
        }

        public IQueryable<TModel> Listar()
        {
            return _dbSet
                .AsNoTracking()
                .OrderBy(x => x.DataInclusao)
                .Where(x => x.Ativo);
        }

        public void Alterar(TModel obj)
        {
            obj.DataAlteracao = DateTime.Now;

            _dbSet.Update(obj);
        }

        public void Remover(Guid id)
        {
            TModel obj = _dbSet.Find(id);

            obj.Ativo = false;
            obj.DataExclusao = DateTime.Now;

            _dbSet.Update(obj);
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
