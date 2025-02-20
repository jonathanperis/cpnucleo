namespace Integration.Tests.Hosts;

public class WebAppFixture : IAsyncLifetime
{
    public IAlbaHost AlbaHost = null!;
    
    public async Task InitializeAsync()
    {
        var securityStub = new JwtSecurityStub();
        
        AlbaHost = await Alba.AlbaHost.For<Program>(securityStub);
    }

    public async Task DisposeAsync()
    {
        await AlbaHost.DisposeAsync();
    }
}