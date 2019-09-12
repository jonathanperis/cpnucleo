using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Infra.Data.Context;
using System;

namespace Cpnucleo.Infra.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly CpnucleoContext Db;

        public UnitOfWork(CpnucleoContext context) => Db = context;

        public bool Commit()
        {
            return Db.SaveChanges() > 0;
        }

        public void Dispose()
        {
            Db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
