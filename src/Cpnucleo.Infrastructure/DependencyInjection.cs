namespace Cpnucleo.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        services.AddScoped<ICryptographyManager, CryptographyManager>();

        services.AddSignalR()
                .AddAzureSignalR(configuration["AzureSignalR_DefaultConnection"]);

        services.AddScoped<IEventManager, EventManager>();

        services.AddServiceBus<PayloadSerializer>(settings =>
        {
            settings.Enabled = true;
            settings.ReceiveMessages = true;
            settings.WithConnection(configuration["AzureServiceBus_DefaultConnection"], new ServiceBusClientOptions());
        });
    }
}