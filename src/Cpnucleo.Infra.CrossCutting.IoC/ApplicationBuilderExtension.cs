using Cpnucleo.Application.Configuration;
using Microsoft.AspNetCore.Builder;

namespace Cpnucleo.Infra.CrossCutting.IoC;

public static class ApplicationBuilderExtension
{
    public static IApplicationBuilder UseCpnucleoApiSetup(this IApplicationBuilder app)
    {
        app.UseApplicationSetup();

        return app;
    }
}