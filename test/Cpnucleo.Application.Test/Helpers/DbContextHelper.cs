using Cpnucleo.Infra.Data.Context;
using Cpnucleo.Infra.Data.UoW;
using Microsoft.EntityFrameworkCore;

namespace Cpnucleo.Application.Test.Helpers;

public class DbContextHelper
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
