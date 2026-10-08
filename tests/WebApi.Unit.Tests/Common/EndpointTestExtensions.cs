namespace WebApi.Unit.Tests.Common;

public static class TestEndpoints
{
    /// <summary>
    /// Creates an endpoint for a handler test. Constructor dependencies are matched by type from
    /// <paramref name="dependencies"/>; a <see cref="ListingChangeNotifier"/> is supplied when none is
    /// given, and is also a request service (FastEndpoints' AddTestServices) for anything resolving it.
    /// </summary>
    public static TEndpoint Create<TEndpoint>(params object[] dependencies) where TEndpoint : class, IEndpoint
    {
        var notifier = dependencies.OfType<ListingChangeNotifier>().FirstOrDefault() ?? new ListingChangeNotifier();
        var arguments = typeof(TEndpoint).GetConstructors().Single().GetParameters()
            .Select(parameter => parameter.ParameterType == typeof(ListingChangeNotifier)
                ? notifier
                : dependencies.FirstOrDefault(parameter.ParameterType.IsInstanceOfType)
                  ?? throw new ArgumentException($"{typeof(TEndpoint).FullName} needs a {parameter.ParameterType.Name}."))
            .ToArray();
        return Factory.Create<TEndpoint>(context => context.AddTestServices(services => services.AddLogging().AddSingleton(notifier)), arguments);
    }
}
