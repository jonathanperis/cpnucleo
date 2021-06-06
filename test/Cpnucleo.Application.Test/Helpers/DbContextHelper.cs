using Cpnucleo.Domain.UoW;
using Cpnucleo.Infra.Data.Context;
using Cpnucleo.Infra.Data.UoW;
using Microsoft.EntityFrameworkCore;
using System;

namespace Cpnucleo.Application.Test.Helpers
{
    internal class DbContextHelper
    {
        public static IUnitOfWork GetContext()
        {
            DbContextOptions<CpnucleoContext> options = new DbContextOptionsBuilder<CpnucleoContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            CpnucleoContext context = new CpnucleoContext(options);
            context.SaveChanges();

            UnitOfWork unitOfWork = new UnitOfWork(context);

            return unitOfWork;
        }
    }
}
