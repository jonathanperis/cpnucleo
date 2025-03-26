using MudBlazor;
using MudBlazor.Services;
using MudBlazor.Translations;
using WebClient.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Add MudBlazor services
builder.Services.AddMudServices();
builder.Services.AddMudTranslations();

// Send all exceptions to the console
MudGlobal.UnhandledExceptionHandler = Console.WriteLine;

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode();

app.Run();
