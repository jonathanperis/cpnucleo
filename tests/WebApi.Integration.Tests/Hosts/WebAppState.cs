namespace WebApi.Integration.Tests.Hosts;

public class WebAppState : StateFixture
{
    public Guid OrganizationId { get; set; }
    public Guid ProjectId { get; set; }
    public Guid WorkflowId { get; set; } 
    public Guid ImpedimentId { get; set; }
    public Guid UserId { get; set; }
    public Guid UserProjectId { get; set; }
    public Guid AssignmentTypeId { get; set; }
    public Guid AssignmentId { get; set; }
    public Guid AssignmentImpedimentId { get; set; }
    public Guid UserAssignmentId { get; set; }
    public Guid AppointmentId { get; set; }
    
    protected override async ValueTask SetupAsync()
    {
        OrganizationId = Guid.NewGuid();
        ProjectId = Guid.NewGuid();
        WorkflowId = Guid.NewGuid();
        ImpedimentId = Guid.NewGuid();
        UserId = Guid.NewGuid();
        UserProjectId = Guid.NewGuid();
        AssignmentTypeId = Guid.NewGuid();
        AssignmentId = Guid.NewGuid();
        AssignmentImpedimentId = Guid.NewGuid();
        UserAssignmentId = Guid.NewGuid();
        AppointmentId = Guid.NewGuid();
        await ValueTask.CompletedTask;
    }

    protected override async ValueTask TearDownAsync()
    {
        OrganizationId = Guid.Empty;
        ProjectId = Guid.Empty;
        WorkflowId = Guid.Empty;
        ImpedimentId = Guid.Empty;
        UserId = Guid.Empty;
        UserProjectId = Guid.Empty;
        AssignmentTypeId = Guid.Empty;
        AssignmentId = Guid.Empty;
        AssignmentImpedimentId = Guid.Empty;
        UserAssignmentId = Guid.Empty;
        AppointmentId = Guid.Empty;
        await ValueTask.CompletedTask;
    }    
}