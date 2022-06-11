using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Cpnucleo.Infra.Data.Context;

internal class CpnucleoContextFactory : IDesignTimeDbContextFactory<CpnucleoContext>
{
    public CpnucleoContextFactory()
    {

    }

    public CpnucleoContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<CpnucleoContext>();

        optionsBuilder
            .UseSqlite(configuration.GetConnectionString("DefaultConnection"))
            .EnableSensitiveDataLogging();

        return new CpnucleoContext(optionsBuilder.Options);
    }
}