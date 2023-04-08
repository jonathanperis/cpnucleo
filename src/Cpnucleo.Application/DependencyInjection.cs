using Cpnucleo.Application.Common.Behaviors;
using Cpnucleo.Application.Common.Hubs;
using Cpnucleo.Application.Events.RemoveSistema;
using Cpnucleo.Shared.Events.RemoveSistema;
using Ev.ServiceBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Cpnucleo.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.RegisterServiceBusReception().FromQueue("CpnucleoDefaultQueue", builder =>
        {
            builder.RegisterReception<RemoveSistemaEvent, RemoveSistemaEventHandler>();
        });

        services.RegisterServiceBusDispatch().ToQueue("CpnucleoDefaultQueue", builder =>
        {
            builder.RegisterDispatch<RemoveSistemaEvent>();
        });
    }

    public static void UseApplication(this IApplicationBuilder app)
    {
        app.UseFileServer();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<ApplicationHub>("/hub");
        });
    }
}
