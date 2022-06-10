using Cpnucleo.Domain.Services;
using Cpnucleo.Domain.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Cpnucleo.Domain;

public static class DependencyInjection
{
    public static void AddDomain(this IServiceCollection services)
    {
        services.AddScoped<IWorkflowService, WorkflowService>();
    }
}