using Azure.Messaging.ServiceBus;
using Cpnucleo.Application.Common.Bus;
using Cpnucleo.Application.Common.Context;
using Cpnucleo.Application.Common.Security;
using Cpnucleo.Infrastructure.Common.Bus;
using Cpnucleo.Infrastructure.Common.Context;
using Cpnucleo.Infrastructure.Common.Security;
using Ev.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EventHandler = Cpnucleo.Infrastructure.Common.Bus.EventHandler;

namespace Cpnucleo.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        services.AddScoped<ICryptographyManager, CryptographyManager>();

        services.AddSignalR()
                .AddAzureSignalR(configuration["AzureSignalR_DefaultConnection"]);

        services.AddScoped<IEventHandler, EventHandler>();

        services.AddServiceBus<PayloadSerializer>(settings =>
        {
            settings.Enabled = true;
            settings.ReceiveMessages = true;
            settings.WithConnection(configuration["AzureServiceBus_DefaultConnection"], new ServiceBusClientOptions());
        });
    }
}