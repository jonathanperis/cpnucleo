using Cpnucleo.Domain.UoW;
using Cpnucleo.Infra.Data.Context;
using Cpnucleo.Infra.Data.UoW;
using Microsoft.EntityFrameworkCore;
using System;

namespace Cpnucleo.Application.Test
{
    public class DbContextHelper
    {
        public static IUnitOfWork GetContext()
        {
            var options = new DbContextOptionsBuilder<CpnucleoContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new CpnucleoContext(options);
            context.SaveChanges();

            var unitOfWork = new UnitOfWork(context);

            return unitOfWork;
        }
    }
}
