using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Cpnucleo.Domain.Configuration
{
    public static class DomainConfig
    {
        public static void AddDomainSetup(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}
