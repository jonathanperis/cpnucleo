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
                .FirstOrDefault(x => x.Id == id && x.Ativo);
        }

        public IQueryable<TModel> Listar()
        {
            return DbSet
                .AsNoTracking()
                .Where(x => x.Ativo);
        }

        public void Alterar(TModel obj)
        {
            obj.Ativo = true;
            obj.DataAlteracao = DateTime.Now;

            DbSet.Update(obj);
        }

        public void Remover(Guid id)
        {
            TModel obj = DbSet.Find(id);
            obj.Ativo = false;
            obj.DataExclusao = DateTime.Now;

            DbSet.Update(obj);
        }

        public void Dispose()
        {
            Db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
