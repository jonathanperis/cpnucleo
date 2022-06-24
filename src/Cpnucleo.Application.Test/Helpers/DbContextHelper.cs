﻿using Cpnucleo.Infrastructure.Data.Context;
using Cpnucleo.Infrastructure.Data.UoW;
using Microsoft.EntityFrameworkCore;

namespace Cpnucleo.Application.Test.Helpers;

public class DbContextHelper
{
    public static IUnitOfWork GetContext()
    {
        DbContextOptions<CpnucleoContext> options = new DbContextOptionsBuilder<CpnucleoContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        CpnucleoContext context = new(options);
        context.SaveChanges();

        UnitOfWork unitOfWork = new(context);

        return unitOfWork;
    }
}
