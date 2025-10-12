var builder = WebApplication.CreateBuilder(args);

builder.ConfigureOpenTelemetry();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Add MudBlazor services
builder.Services.AddMudServices();
builder.Services.AddMudTranslations();

// Send all exceptions to the console
MudGlobal.UnhandledExceptionHandler = Console.WriteLine;

// Configure HttpClient and WebApi.Client
builder.Services.AddHttpClient<Cpnucleo.WebApi.Client.WebApiClient>((serviceProvider, client) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var baseUrl = configuration.GetValue<string>("WebApiBaseUrl") ?? "http://localhost:5020";
    client.BaseAddress = new Uri(baseUrl);
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
});

builder.Services.AddScoped<Cpnucleo.WebApi.Client.WebApiClient>(sp =>
{
    var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient(nameof(Cpnucleo.WebApi.Client.WebApiClient));
    var requestAdapter = new Microsoft.Kiota.Http.HttpClientLibrary.HttpClientRequestAdapter(
        new Microsoft.Kiota.Abstractions.Authentication.AnonymousAuthenticationProvider(),
        httpClient: httpClient);
    return new Cpnucleo.WebApi.Client.WebApiClient(requestAdapter);
});

builder.Services.AddHealthChecks();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.MapHealthChecks("/healthz");

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode();

app.Run();
