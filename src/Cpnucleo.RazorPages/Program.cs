using Cpnucleo.RazorPages.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using StackExchange.Profiling.Storage;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPagesConfigSetup(builder.Configuration);

builder.Services.AddAntiforgery(options => options.HeaderName = "XSRF-TOKEN");

builder.Services.Configure<ApplicationConfigurations>(
    builder.Configuration.GetSection("ApplicationConfigurations"));

builder.Services
    .AddAuthentication(x =>
    {
        x.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        x.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        x.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(x =>
    {
        x.LoginPath = new PathString("/Login");
        x.AccessDeniedPath = new PathString("/Negado");
    });

builder.Services.AddMiniProfiler(options =>
{
    // All of this is optional. You can simply call .AddMiniProfiler() for all defaults

    // (Optional) Path to use for profiler URLs, default is /mini-profiler-resources
    options.RouteBasePath = "/profiler";

    // (Optional) Control storage
    // (default is 30 minutes in MemoryCacheStorage)
    (options.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(60);

    // (Optional) You can disable "Connection Open()", "Connection Close()" (and async variant) tracking.
    // (defaults to true, and connection opening/closing is tracked)
    options.TrackConnectionOpenClose = true;
});

// Add services to the container.
builder.Services.AddRazorPages().AddRazorPagesOptions(options =>
{
    options.Conventions.AddPageRoute("/Login", "");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

RequestLocalizationOptions localizationOptions = new RequestLocalizationOptions
{
    SupportedCultures = new List<CultureInfo> { new CultureInfo("pt-BR") },
    SupportedUICultures = new List<CultureInfo> { new CultureInfo("pt-BR") },
    DefaultRequestCulture = new RequestCulture("pt-BR")
};

app.UseMiniProfiler();

app.UseRequestLocalization(localizationOptions);
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();