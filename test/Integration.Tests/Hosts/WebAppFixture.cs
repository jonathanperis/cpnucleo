namespace Integration.Tests.Hosts;

public class WebAppFixture : IAsyncLifetime
{
    public IAlbaHost AlbaHost = null!;
    public Guid OrganizationId { get; set; } = Guid.NewGuid();
    public Guid ProjectId { get; set; } = Guid.NewGuid();
    public Guid WorkflowId { get; set; } = Guid.NewGuid();
    public Guid ImpedimentId { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; } = Guid.NewGuid();
    public Guid UserProjectId { get; set; } = Guid.NewGuid();
    public Guid AssignmentTypeId { get; set; } = Guid.NewGuid();
    public Guid AssignmentId { get; set; } = Guid.NewGuid();
    public Guid AssignmentImpedimentId { get; set; } = Guid.NewGuid();
    public Guid UserAssignmentId { get; set; } = Guid.NewGuid();
    public Guid AppointmentId { get; set; } = Guid.NewGuid();
    
    public async Task InitializeAsync()
    {
        var securityStub = new JwtSecurityStub();
        
        const string dbConnectionString = 
            "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=cpnucleo;Minimum Pool Size=10;Maximum Pool Size=10;Multiplexing=true";
        
        AlbaHost = await Alba.AlbaHost.For<Program>(builder =>
        {
            builder.UseSetting("OTEL_EXPORTER_OTLP_ENDPOINT", "http://localhost:4317");
            builder.UseSetting("OTEL_METRIC_EXPORT_INTERVAL", "100");
            builder.UseSetting("DB_CONNECTION_STRING", dbConnectionString);
        }, securityStub);
    }

    public async Task DisposeAsync()
    {
        await AlbaHost.DisposeAsync();
    }
}