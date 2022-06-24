using Azure.Messaging.ServiceBus;
using Cpnucleo.Infrastructure.Bus.Interfaces;
using Cpnucleo.Shared.Events.Sistema;
using Ev.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cpnucleo.Infrastructure.Bus;

public static class DependencyInjection
{
    public static void AddInfraCrossCuttingBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEventHandler, EventHandler>();

        services.AddServiceBus<PayloadSerializer>(settings =>
        {
            settings.Enabled = true;
            settings.ReceiveMessages = true;
            settings.WithConnection(configuration["AzureServiceBus:DefaultConnection"], new ServiceBusClientOptions());
        });

        services.RegisterServiceBusDispatch().ToQueue("CpnucleoDefaultQueue", builder =>
        {
            builder.RegisterDispatch<RemoveSistemaEvent>();
        });
    }
}