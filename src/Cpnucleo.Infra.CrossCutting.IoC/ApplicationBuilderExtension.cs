namespace Cpnucleo.Infra.CrossCutting.IoC;

using Microsoft.AspNetCore.Builder;

public static class ApplicationBuilderExtension
{
    public static IApplicationBuilder UseCpnucleoApiSetup(this IApplicationBuilder app)
    {
        return app;
    }
}