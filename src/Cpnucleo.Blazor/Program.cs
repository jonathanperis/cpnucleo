using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Cpnucleo.Blazor.Services.Interfaces;
using Refit;

namespace Cpnucleo.Blazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services
                .AddRefitClient<ICpnucleoApiService>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri($"{builder.Configuration.GetValue<string>("AppSettings:UrlCpnucleoApi")}/api/v2"));

            await builder.Build().RunAsync();       
        }
    }
}
