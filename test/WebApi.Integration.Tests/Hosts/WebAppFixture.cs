namespace WebApi.Integration.Tests.Hosts;

public class WebAppFixture : AppFixture<Program>
{
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

    // protected override ValueTask SetupAsync()
    // {
    //     // place one-time setup code here
    // }

    // protected override void ConfigureApp(IWebHostBuilder a)
    // {
    //     // do host builder config here
    // }

    // protected override void ConfigureServices(IServiceCollection s)
    // {
    //     // do test service registration here
    // }

    // protected override ValueTask TearDownAsync()
    // {
    //     // do cleanups here
    // }
}
