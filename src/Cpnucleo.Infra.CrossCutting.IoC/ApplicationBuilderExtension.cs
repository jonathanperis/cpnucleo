using Microsoft.AspNetCore.Builder;

namespace Cpnucleo.Infra.CrossCutting.IoC
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder UseCpnucleoApiSetup(this IApplicationBuilder app)
        {
            return app;
        }

        public static IApplicationBuilder UseCpnucleoSetup(this IApplicationBuilder app)
        {
            return app;
        }
    }
}
