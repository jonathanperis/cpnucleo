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
        private readonly CpnucleoContext Db;
        protected readonly DbSet<TModel> DbSet;

        public CrudRepository(CpnucleoContext context)
        {
            Db = context;
            DbSet = Db.Set<TModel>();
        }

        public void Incluir(TModel obj)
        {
            DbSet.Add(obj);
        }

        public TModel Consultar(Guid id)
        {
            return DbSet
                .AsNoTracking()
                .Include(Db.GetIncludePaths(typeof(TModel)))
                .SingleOrDefault(x => x.Id == id);
        }

        public IQueryable<TModel> Listar()
        {
            return DbSet
                .AsNoTracking()
                .Where(x => x.Ativo == false);
        }

        public void Alterar(TModel obj)
        {
            DbSet.Update(obj);
        }

        public void Remover(Guid id)
        {
            TModel obj = Consultar(id);
            obj.Ativo = false;

            Alterar(obj);
        }

        public void Dispose()
        {
            Db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
