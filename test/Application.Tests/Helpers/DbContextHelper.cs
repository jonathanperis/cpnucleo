namespace Application.Tests.Helpers;

public class DbContextHelper
{
    public static IApplicationDbContext GetContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(BaseEntity.GetNewId().ToString())
            .Options;

        ApplicationDbContext context = new(options);
        SeedData(context);

        return context;
    }

    private static void SeedData(ApplicationDbContext context)
    {
        var organizationId = BaseEntity.GetNewId();
        context?.Organizations?.Add(Organization.Create("Test Organization 1", "Description 1", organizationId));
        context?.Organizations?.Add(Organization.Create("Test Organization 2", "Description 2", BaseEntity.GetNewId()));
        context?.Organizations?.Add(Organization.Create("Test Organization 3", "Description 3", BaseEntity.GetNewId()));

        var projectId = BaseEntity.GetNewId();
        context?.Projects?.Add(Project.Create("Test Project 1", organizationId, projectId));
        context?.Projects?.Add(Project.Create("Test Project 2", organizationId, BaseEntity.GetNewId()));
        context?.Projects?.Add(Project.Create("Test Project 3", organizationId, BaseEntity.GetNewId()));

        var workflowId = BaseEntity.GetNewId();
        context?.Workflows?.Add(Workflow.Create("Test Workflow 1", 1, workflowId));
        context?.Workflows?.Add(Workflow.Create("Test Workflow 2", 2, BaseEntity.GetNewId()));
        context?.Workflows?.Add(Workflow.Create("Test Workflow 3", 3, BaseEntity.GetNewId()));

        var impedimentId = BaseEntity.GetNewId();
        context?.Impediments?.Add(Impediment.Create("Test Impediment 1", impedimentId));
        context?.Impediments?.Add(Impediment.Create("Test Impediment 2", BaseEntity.GetNewId()));
        context?.Impediments?.Add(Impediment.Create("Test Impediment 3", BaseEntity.GetNewId()));

        var userId = BaseEntity.GetNewId();
        context?.Users?.Add(User.Create("Test User 1", "testUser1", "password1", userId));
        context?.Users?.Add(User.Create("Test User 2", "testUser2", "password2", BaseEntity.GetNewId()));
        context?.Users?.Add(User.Create("Test User 3", "testUser3", "password3", BaseEntity.GetNewId()));

        var assignmentTypeId = BaseEntity.GetNewId();
        context?.AssignmentTypes?.Add(AssignmentType.Create("Test AssignmentType 1", assignmentTypeId));
        context?.AssignmentTypes?.Add(AssignmentType.Create("Test AssignmentType 2", BaseEntity.GetNewId()));
        context?.AssignmentTypes?.Add(AssignmentType.Create("Test AssignmentType 3", BaseEntity.GetNewId()));

        var assignmentId = BaseEntity.GetNewId();
        context?.Assignments?.Add(Assignment.Create("Test Assignment 1", "Description 1", DateTime.UtcNow, DateTime.UtcNow.AddDays(1), 2, projectId, workflowId, userId, assignmentTypeId, assignmentId));
        context?.Assignments?.Add(Assignment.Create("Test Assignment 2", "Description 2", DateTime.UtcNow, DateTime.UtcNow.AddDays(1), 2, projectId, workflowId, userId, assignmentTypeId));
        context?.Assignments?.Add(Assignment.Create("Test Assignment 3", "Description 3", DateTime.UtcNow, DateTime.UtcNow.AddDays(1), 2, projectId, workflowId, userId, assignmentTypeId));

        var appointmentId = BaseEntity.GetNewId();
        context?.Appointments?.Add(Appointment.Create("Test Appointment 1", DateTime.UtcNow, 1, assignmentId, userId, appointmentId));
        context?.Appointments?.Add(Appointment.Create("Test Appointment 2", DateTime.UtcNow, 1, BaseEntity.GetNewId(), BaseEntity.GetNewId()));
        context?.Appointments?.Add(Appointment.Create("Test Appointment 3", DateTime.UtcNow, 1, BaseEntity.GetNewId(), BaseEntity.GetNewId()));

        var assignmentImpedimentId = BaseEntity.GetNewId();
        context?.AssignmentImpediments?.Add(AssignmentImpediment.Create("Test AssignmentImpediment 1", assignmentId, impedimentId, assignmentImpedimentId));
        context?.AssignmentImpediments?.Add(AssignmentImpediment.Create("Test AssignmentImpediment 2", BaseEntity.GetNewId(), BaseEntity.GetNewId()));
        context?.AssignmentImpediments?.Add(AssignmentImpediment.Create("Test AssignmentImpediment 3", BaseEntity.GetNewId(), BaseEntity.GetNewId()));

        var userProjectId = BaseEntity.GetNewId();
        context?.UserProjects?.Add(UserProject.Create(userId, projectId, userProjectId));
        context?.UserProjects?.Add(UserProject.Create(BaseEntity.GetNewId(), BaseEntity.GetNewId(), BaseEntity.GetNewId()));
        context?.UserProjects?.Add(UserProject.Create(BaseEntity.GetNewId(), BaseEntity.GetNewId(), BaseEntity.GetNewId()));

        var userAssignmentId = BaseEntity.GetNewId();
        context?.UserAssignments?.Add(UserAssignment.Create(userId, assignmentId, userAssignmentId));
        context?.UserAssignments?.Add(UserAssignment.Create(BaseEntity.GetNewId(), BaseEntity.GetNewId()));
        context?.UserAssignments?.Add(UserAssignment.Create(BaseEntity.GetNewId(), BaseEntity.GetNewId()));

        context?.SaveChangesAsync();
    }
}