namespace WebApi.Unit.Tests.Common;

public static class EndpointTestExtensions
{
    private static readonly IServiceProvider ListingServices = new ServiceCollection()
        .AddLogging()
        .AddSingleton<ListingChangeNotifier>()
        .BuildServiceProvider();

    public static TEndpoint WithListingServices<TEndpoint>(this TEndpoint endpoint)
    {
        if (endpoint is null) throw new ArgumentNullException(nameof(endpoint));

        var httpContext = (HttpContext?)endpoint.GetType().GetProperty(nameof(HttpContext))?.GetValue(endpoint)
            ?? throw new InvalidOperationException("Endpoint test instance does not expose HttpContext.");

        httpContext.RequestServices = ListingServices;
        return endpoint;
    }
}
