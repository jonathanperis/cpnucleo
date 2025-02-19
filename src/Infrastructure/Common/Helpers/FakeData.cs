namespace Infrastructure.Common.Helpers;

public static class FakeData
{    
    internal static List<Appointment>? Appointments { get; set; }
    internal static List<AssignmentImpediment>? AssignmentImpediments { get; set; }
    internal static List<Assignment>? Assignments { get; set; }
    internal static List<AssignmentType>? AssignmentTypes { get; set; }
    internal static List<Impediment>? Impediments { get; set; }
    internal static List<Organization>? Organizations { get; set; }
    internal static List<Project>? Projects { get; set; }
    internal static List<UserAssignment>? UserAssignments { get; set; }
    internal static List<User>? Users { get; set; }
    internal static List<UserProject>? UserProjects { get; set; }
    internal static List<Workflow>? Workflows { get; set; }
    
    public static void Init()
    {
        var random = new Random();
        
        var organizationFaker = new Faker<Organization>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(x => x.Name, f => string.Concat(f.Hacker.Noun(), f.Hacker.Adjective()))
            .RuleFor(x => x.Description, f => f.Hacker.Phrase())
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(x => x.Active, f => f.Random.Bool());

        Organizations = organizationFaker.Generate(86);

        var projectFaker = new Faker<Project>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(x => x.Name, f => string.Concat(f.Hacker.Noun(), f.Hacker.Adjective()))
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(x => x.Active, f => f.Random.Bool())
            .RuleFor(x => x.OrganizationId, Organizations[random.Next(Organizations.Count)].Id);
            
        Projects = projectFaker.Generate(258);

        var impedimentFaker = new Faker<Impediment>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(x => x.Name, f => string.Concat(f.Hacker.Noun(), f.Hacker.Adjective()))
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(x => x.Active, f => f.Random.Bool());
            
        Impediments = impedimentFaker.Generate(14);
        
        var assignmentTypeFaker = new Faker<AssignmentType>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(x => x.Name, f => string.Concat(f.Hacker.Noun(), f.Hacker.Adjective()))
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(x => x.Active, f => f.Random.Bool());
            
        AssignmentTypes = assignmentTypeFaker.Generate(14);
        
        var workflowFaker = new Faker<Workflow>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(x => x.Name, f => string.Concat(f.Hacker.Noun(), f.Hacker.Adjective()))
            .RuleFor(x => x.Order, f => f.IndexGlobal + 1)
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(x => x.Active, f => f.Random.Bool());
            
        Workflows = workflowFaker.Generate(6);        
        
        var userFaker = new Faker<User>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(x => x.Name, f => f.Name.FullName())
            .RuleFor(x => x.Login, f => f.Internet.UserName())
            .RuleFor(x => x.Password, f => f.Internet.Password())
            .RuleFor(x => x.Salt, f => f.Internet.Password())
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(x => x.Active, f => f.Random.Bool());
            
        Users = userFaker.Generate(1154);         
        
        var userProjectFaker = new Faker<UserProject>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(x => x.Active, f => f.Random.Bool())
            .RuleFor(x => x.UserId, Users[random.Next(Users.Count)].Id)
            .RuleFor(x => x.ProjectId, Projects[random.Next(Projects.Count)].Id);
            
        UserProjects = userProjectFaker.Generate(4400);

        var assignmentFaker = new Faker<Assignment>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(x => x.Name, f => string.Concat(f.Hacker.Noun(), f.Hacker.Adjective()))
            .RuleFor(x => x.Description, f => f.Hacker.Phrase())
            .RuleFor(o => o.StartDate, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.EndDate, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(x => x.AmountHours, f => f.Random.Number(12, 60))
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(x => x.Active, f => f.Random.Bool())
            .RuleFor(x => x.ProjectId, Projects[random.Next(Projects.Count)].Id)
            .RuleFor(x => x.WorkflowId, Workflows[random.Next(Workflows.Count)].Id)
            .RuleFor(x => x.UserId, Users[random.Next(Users.Count)].Id)
            .RuleFor(x => x.AssignmentTypeId, AssignmentTypes[random.Next(AssignmentTypes.Count)].Id);

        Assignments = assignmentFaker.Generate(64587);    
        
        var userAssignmentFaker = new Faker<UserAssignment>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(x => x.Active, f => f.Random.Bool())
            .RuleFor(x => x.UserId, Users[random.Next(Users.Count)].Id)
            .RuleFor(x => x.AssignmentId, Assignments[random.Next(Assignments.Count)].Id);
            
        UserAssignments = userAssignmentFaker.Generate(63554);    
        
        var assignmentImpedimentFaker = new Faker<AssignmentImpediment>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(x => x.Active, f => f.Random.Bool())
            .RuleFor(x => x.ImpedimentId, Impediments[random.Next(Impediments.Count)].Id)
            .RuleFor(x => x.AssignmentId, Assignments[random.Next(Assignments.Count)].Id);
            
        AssignmentImpediments = assignmentImpedimentFaker.Generate(1369);     
        
        var appointmentFaker = new Faker<Appointment>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(x => x.Description, f => f.Hacker.Phrase())
            .RuleFor(x => x.AmountHours, f => f.Random.Number(01, 06))
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(x => x.Active, f => f.Random.Bool())
            .RuleFor(x => x.AssignmentId, Assignments[random.Next(Assignments.Count)].Id)
            .RuleFor(x => x.UserId, Users[random.Next(Users.Count)].Id);

        Appointments = appointmentFaker.Generate(89571);        
    }
}