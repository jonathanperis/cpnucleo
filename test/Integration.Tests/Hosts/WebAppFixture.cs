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
        
        AlbaHost = await Alba.AlbaHost.For<WebApi.Program2>(securityStub);
    }

    public async Task DisposeAsync()
    {
        await AlbaHost.DisposeAsync();
    }
}