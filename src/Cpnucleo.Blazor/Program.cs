using Cpnucleo.Blazor.Services.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Refit;

namespace Cpnucleo.Blazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services
                .AddRefitClient<ICpnucleoApiService>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri($"{builder.Configuration.GetValue<string>("AppSettings:UrlCpnucleoApi")}/api/v2"));

            await builder.Build().RunAsync();
        }
    }
}
