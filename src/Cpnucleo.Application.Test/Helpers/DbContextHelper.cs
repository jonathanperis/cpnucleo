using Cpnucleo.Infrastructure.Context;
using Cpnucleo.Infrastructure.UoW;
using Microsoft.EntityFrameworkCore;

namespace Cpnucleo.Application.Test.Helpers;

public class DbContextHelper
{
    public static IUnitOfWork GetContext()
    {
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        ApplicationDbContext context = new(options);
        context.SaveChanges();

        UnitOfWork unitOfWork = new(context);

        return unitOfWork;
    }
}
