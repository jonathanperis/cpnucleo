namespace Cpnucleo.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediator(options =>
        {
            options.ServiceLifetime = ServiceLifetime.Scoped;
        });

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));

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
