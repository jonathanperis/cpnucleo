using Cpnucleo.Infrastructure.Context;
using Cpnucleo.Infrastructure.UoW;
using Microsoft.EntityFrameworkCore;

namespace Cpnucleo.Application.Test.Helpers;

public class DbContextHelper
{
    public static IUnitOfWork GetContext()
    {
        DbContextOptions<CpnucleoDbContext> options = new DbContextOptionsBuilder<CpnucleoDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        CpnucleoDbContext context = new(options);
        context.SaveChanges();

        UnitOfWork unitOfWork = new(context);

        return unitOfWork;
    }
}
