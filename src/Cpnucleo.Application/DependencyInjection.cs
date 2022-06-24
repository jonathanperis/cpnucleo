using Cpnucleo.Application.Common.Behaviors;
using Cpnucleo.Application.Common.Security;
using Cpnucleo.Application.Common.Security.Interfaces;
using Cpnucleo.Application.Events.Sistema;
using Cpnucleo.Application.Hubs;
using Cpnucleo.Shared.Events.Sistema;
using Ev.ServiceBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Cpnucleo.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddScoped<ICryptographyManager, CryptographyManager>();

        services.RegisterServiceBusReception().FromQueue("CpnucleoDefaultQueue", builder =>
        {
            builder.RegisterReception<RemoveSistemaEvent, RemoveSistemaHandler>();
        });

        services.AddSignalR()
                .AddAzureSignalR(configuration["AzureSignalR:DefaultConnection"]);
    }

    public static void UseApplication(this IApplicationBuilder app)
    {
        app.UseFileServer();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<CpnucleoHub>("/hub");
        });
    }
}
