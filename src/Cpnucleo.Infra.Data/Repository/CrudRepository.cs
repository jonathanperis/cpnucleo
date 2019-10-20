using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
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
            _dbSet.Add(obj);
        }

        public TModel Consultar(Guid id)
        {
            return _dbSet
                .AsNoTracking()
                .Include(_context.GetIncludePaths(typeof(TModel)))
                .FirstOrDefault(x => x.Id == id && x.Ativo);
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
            obj.Ativo = true;
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
