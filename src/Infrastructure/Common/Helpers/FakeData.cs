namespace Infrastructure.Common.Helpers;

internal static class FakeData
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
    
    internal static void Init()
    {
        var random = new Random();
        var sb = new StringBuilder();
        
        var organizationFaker = new Faker<Organization>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(x => x.Name, f => string.Format("{0} {1} {2}", f.Hacker.Noun(), f.Hacker.IngVerb(), f.Hacker.Adjective()))
            .RuleFor(x => x.Description, f => f.Hacker.Phrase())
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(x => x.Active, f => f.Random.Bool());

        Organizations = organizationFaker.Generate(686);
        int lastIndex = Organizations.Count - 1;
        int currentIndex = 0;
        
        sb.AppendLine("""
                        INSERT INTO "Organizations" ("Id", "Name", "Description", "CreatedAt", "UpdatedAt", "Active") VALUES 
                        """);
        foreach (var item in Organizations)
        {
            bool isLast = currentIndex == lastIndex;
            sb.AppendLine($"""
                           ('{item.Id}'::UUID, '{item.Name?.Replace("'", "''")}', '{item.Description?.Replace("'", "''")}', '{item.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")}', {item.Active.ToString().ToLower()}){(isLast ? ";" : ",")}
                           """);
            currentIndex++;
        }

        var projectFaker = new Faker<Project>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(x => x.Name, f => string.Format("{0} {1} {2}", f.Hacker.Noun(), f.Hacker.IngVerb(), f.Hacker.Adjective()))
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(x => x.Active, f => f.Random.Bool())
            .RuleFor(x => x.OrganizationId, Organizations[random.Next(Organizations.Count)].Id);
            
        Projects = projectFaker.Generate(1258);
        lastIndex = Projects.Count - 1;
        currentIndex = 0;

        sb.AppendLine();
        sb.AppendLine("""
                        INSERT INTO "Projects" ("Id", "Name", "OrganizationId", "CreatedAt", "UpdatedAt", "Active") VALUES  
                        """);        
        foreach (var item in Projects)
        {
            bool isLast = currentIndex == lastIndex;
            sb.AppendLine($"""
                           ('{item.Id}'::UUID, '{item.Name?.Replace("'", "''")}', '{item.OrganizationId}'::UUID, '{item.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")}', {item.Active.ToString().ToLower()}){(isLast ? ";" : ",")}
                           """);
            currentIndex++;
        }

        var impedimentFaker = new Faker<Impediment>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(x => x.Name, f => string.Format("{0} {1} {2}", f.Hacker.Noun(), f.Hacker.IngVerb(), f.Hacker.Adjective()))
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(x => x.Active, f => f.Random.Bool());
            
        Impediments = impedimentFaker.Generate(114);
        lastIndex = Impediments.Count - 1;
        currentIndex = 0;  

        sb.AppendLine();
        sb.AppendLine("""
                        INSERT INTO "Impediments" ("Id", "Name", "CreatedAt", "UpdatedAt", "Active") VALUES  
                        """);        
        foreach (var item in Impediments)
        {
            bool isLast = currentIndex == lastIndex;
            sb.AppendLine($"""
                           ('{item.Id}'::UUID, '{item.Name?.Replace("'", "''")}', '{item.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")}', {item.Active.ToString().ToLower()}){(isLast ? ";" : ",")}
                           """);
            currentIndex++;
        }

        var assignmentTypeFaker = new Faker<AssignmentType>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(x => x.Name, f => string.Format("{0} {1} {2}", f.Hacker.Noun(), f.Hacker.IngVerb(), f.Hacker.Adjective()))
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(x => x.Active, f => f.Random.Bool());
            
        AssignmentTypes = assignmentTypeFaker.Generate(3);
        lastIndex = AssignmentTypes.Count - 1;
        currentIndex = 0;  

        sb.AppendLine();
        sb.AppendLine("""
                        INSERT INTO "AssignmentTypes" ("Id", "Name", "CreatedAt", "UpdatedAt", "Active") VALUES  
                        """);        
        foreach (var item in AssignmentTypes)
        {
            bool isLast = currentIndex == lastIndex;
            sb.AppendLine($"""
                           ('{item.Id}'::UUID, '{item.Name?.Replace("'", "''")}', '{item.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")}', {item.Active.ToString().ToLower()}){(isLast ? ";" : ",")}
                           """);
            currentIndex++;
        }

        var workflowFaker = new Faker<Workflow>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(x => x.Name, f => string.Format("{0} {1} {2}", f.Hacker.Noun(), f.Hacker.IngVerb(), f.Hacker.Adjective()))
            .RuleFor(x => x.Order, f => f.IndexGlobal + 1)
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(x => x.Active, f => f.Random.Bool());
            
        Workflows = workflowFaker.Generate(6);        
        lastIndex = Workflows.Count - 1;
        currentIndex = 0;  

        sb.AppendLine();
        sb.AppendLine("""
                        INSERT INTO "Workflows" ("Id", "Name", "Order", "CreatedAt", "UpdatedAt", "Active") VALUES  
                        """);        
        foreach (var item in Workflows)
        {
            bool isLast = currentIndex == lastIndex;
            sb.AppendLine($"""
                           ('{item.Id}'::UUID, '{item.Name?.Replace("'", "''")}', {item.Order}, '{item.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")}', {item.Active.ToString().ToLower()}){(isLast ? ";" : ",")}
                           """);
            currentIndex++;
        }

        var userFaker = new Faker<User>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(x => x.Name, f => f.Name.FullName())
            .RuleFor(x => x.Login, f => f.Internet.UserName())
            .RuleFor(x => x.Password, f => string.Concat(f.Internet.Password(), f.Internet.Password(), f.Internet.Password()))
            .RuleFor(x => x.Salt, f => f.Internet.Password())
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(x => x.Active, f => f.Random.Bool());
            
        Users = userFaker.Generate(11154);         
        lastIndex = Users.Count - 1;
        currentIndex = 0;  

        sb.AppendLine();
        sb.AppendLine("""
                        INSERT INTO "Users" ("Id", "Name", "Login", "Password", "Salt", "CreatedAt", "UpdatedAt", "Active") VALUES  
                        """);        
        foreach (var item in Users)
        {
            bool isLast = currentIndex == lastIndex;
            sb.AppendLine($"""
                           ('{item.Id}'::UUID, '{item.Name?.Replace("'", "''")}', '{item.Login}', '{item.Password}', '{item.Salt}', '{item.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")}', {item.Active.ToString().ToLower()}){(isLast ? ";" : ",")}
                           """);
            currentIndex++;
        }

        var userProjectFaker = new Faker<UserProject>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(x => x.Active, f => f.Random.Bool())
            .RuleFor(x => x.UserId, Users[random.Next(Users.Count)].Id)
            .RuleFor(x => x.ProjectId, Projects[random.Next(Projects.Count)].Id);
            
        UserProjects = userProjectFaker.Generate(24400);
        lastIndex = UserProjects.Count - 1;
        currentIndex = 0;  

        sb.AppendLine();
        sb.AppendLine("""
                        INSERT INTO "UserProjects" ("Id", "UserId", "ProjectId", "CreatedAt", "UpdatedAt", "Active") VALUES  
                        """);        
        foreach (var item in UserProjects)
        {
            bool isLast = currentIndex == lastIndex;
            sb.AppendLine($"""
                           ('{item.Id}'::UUID, '{item.UserId}'::UUID, '{item.ProjectId}'::UUID, '{item.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")}', {item.Active.ToString().ToLower()}){(isLast ? ";" : ",")}
                           """);
            currentIndex++;
        }

        var assignmentFaker = new Faker<Assignment>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(x => x.Name, f => string.Format("{0} {1} {2}", f.Hacker.Noun(), f.Hacker.IngVerb(), f.Hacker.Adjective()))
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

        Assignments = assignmentFaker.Generate(464587);    
        lastIndex = Assignments.Count - 1;
        currentIndex = 0;  

        sb.AppendLine();
        sb.AppendLine("""
                        INSERT INTO "Assignments" ("Id", "Name", "Description", "StartDate", "EndDate", "AmountHours", "ProjectId", "WorkflowId", "UserId", "AssignmentTypeId", "CreatedAt", "UpdatedAt", "Active") VALUES  
                        """);        
        foreach (var item in Assignments)
        {
            bool isLast = currentIndex == lastIndex;
            sb.AppendLine($"""
                            ('{item.Id}'::UUID, '{item.Name?.Replace("'", "''")}', '{item.Description?.Replace("'", "''")}', '{item.StartDate.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.EndDate.ToString("yyyy-MM-dd HH:mm:ss")}', {item.AmountHours}, '{item.ProjectId}'::UUID, '{item.WorkflowId}'::UUID, '{item.UserId}'::UUID, '{item.AssignmentTypeId}'::UUID, '{item.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")}', {item.Active.ToString().ToLower()}){(isLast ? ";" : ",")}
                            """);
            currentIndex++;
        }

        var userAssignmentFaker = new Faker<UserAssignment>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(x => x.Active, f => f.Random.Bool())
            .RuleFor(x => x.UserId, Users[random.Next(Users.Count)].Id)
            .RuleFor(x => x.AssignmentId, Assignments[random.Next(Assignments.Count)].Id);
            
        UserAssignments = userAssignmentFaker.Generate(363554);    
        lastIndex = UserAssignments.Count - 1;
        currentIndex = 0;  

        sb.AppendLine();
        sb.AppendLine("""
                        INSERT INTO "UserAssignments" ("Id", "UserId", "AssignmentId", "CreatedAt", "UpdatedAt", "Active") VALUES  
                        """);        
        foreach (var item in UserAssignments)
        {
            bool isLast = currentIndex == lastIndex;
            sb.AppendLine($"""
                           ('{item.Id}'::UUID, '{item.UserId}'::UUID, '{item.AssignmentId}'::UUID, '{item.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")}', {item.Active.ToString().ToLower()}){(isLast ? ";" : ",")}
                           """);
            currentIndex++;
        }

        var assignmentImpedimentFaker = new Faker<AssignmentImpediment>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(x => x.Active, f => f.Random.Bool())
            .RuleFor(x => x.ImpedimentId, Impediments[random.Next(Impediments.Count)].Id)
            .RuleFor(x => x.AssignmentId, Assignments[random.Next(Assignments.Count)].Id);
            
        AssignmentImpediments = assignmentImpedimentFaker.Generate(11369);     
        lastIndex = AssignmentImpediments.Count - 1;
        currentIndex = 0;  

        sb.AppendLine();
        sb.AppendLine("""
                        INSERT INTO "AssignmentImpediments" ("Id", "Description", "AssignmentId", "ImpedimentId", "CreatedAt", "UpdatedAt", "Active") VALUES  
                        """);        
        foreach (var item in AssignmentImpediments)
        {
            bool isLast = currentIndex == lastIndex;
            sb.AppendLine($"""
                           ('{item.Id}'::UUID, '{item.Description?.Replace("'", "''")}', '{item.AssignmentId}'::UUID, '{item.ImpedimentId}'::UUID, '{item.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")}', {item.Active.ToString().ToLower()}){(isLast ? ";" : ",")}
                           """);
            currentIndex++;
        }

        var appointmentFaker = new Faker<Appointment>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(x => x.Description, f => f.Hacker.Phrase())
            .RuleFor(o => o.KeepDate, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -9)), DateTime.UtcNow.AddMonths(f.Random.Number(-8, -6))))            
            .RuleFor(x => x.AmountHours, f => f.Random.Number(01, 06))
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(x => x.Active, f => f.Random.Bool())
            .RuleFor(x => x.AssignmentId, Assignments[random.Next(Assignments.Count)].Id)
            .RuleFor(x => x.UserId, Users[random.Next(Users.Count)].Id);

        Appointments = appointmentFaker.Generate(489571);      
        lastIndex = Appointments.Count - 1;
        currentIndex = 0;  

        sb.AppendLine();
        sb.AppendLine("""
                        INSERT INTO "Appointments" ("Id", "Description", "KeepDate", "AmountHours", "AssignmentId", "UserId", "CreatedAt", "UpdatedAt", "Active") VALUES  
                        """);        
        foreach (var item in Appointments)
        {
            bool isLast = currentIndex == lastIndex;
            sb.AppendLine($"""
                           ('{item.Id}'::UUID, '{item.Description?.Replace("'", "''")}', '{item.KeepDate.ToString("yyyy-MM-dd HH:mm:ss")}', {item.AmountHours}, '{item.AssignmentId}'::UUID, '{item.UserId}'::UUID, '{item.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")}', {item.Active.ToString().ToLower()}){(isLast ? ";" : ",")}
                           """);
            currentIndex++;
        }     

        string filePath = "003-database-dump-dml.sql";

        // It will write in the root project folder
        File.WriteAllText(filePath, sb.ToString());             
    }
}