namespace Infrastructure.Common.Helpers;

internal static class FakeDataHelper
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
    
    internal static void CreateSqlDumpFile()
    {
        var random = new Random();
        var sb = new StringBuilder();
        
        var organizationFaker = new Faker<Organization>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(x => x.Name, f => string.Format("{0} {1} {2}", f.Hacker.Noun(), f.Hacker.IngVerb(), f.Hacker.Adjective()))
            .RuleFor(x => x.Description, f => f.Hacker.Phrase())
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(o => o.DeletedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(f.Random.Number(-7, -6))))
            .RuleFor(o => o.DeletedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(f.Random.Number(-7, -6))))
            .RuleFor(x => x.Active, f => f.Random.Bool());

        Organizations = organizationFaker.Generate(686);
        int lastIndex = Organizations.Count - 1;
        int currentIndex = 0;
        
        sb.AppendLine("""
                        INSERT INTO "Organizations" ("Id", "Name", "Description", "CreatedAt", "UpdatedAt", "DeletedAt", "Active") VALUES 
                        """);
        foreach (var item in Organizations)
        {
            bool isLast = currentIndex == lastIndex;
            sb.AppendLine($"""
                           ('{item.Id}'::UUID, '{item.Name?.Replace("'", "''")}', '{item.Description?.Replace("'", "''")}', '{item.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")}', '{(!item.Active ? item.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") : null)}', {item.Active.ToString().ToLower()}){(isLast ? ";" : ",")}
                           """);
            currentIndex++;
        }

        var projectFaker = new Faker<Project>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(x => x.Name, f => string.Format("{0} {1} {2}", f.Hacker.Noun(), f.Hacker.IngVerb(), f.Hacker.Adjective()))
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(o => o.DeletedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(f.Random.Number(-7, -6))))
            .RuleFor(x => x.Active, f => f.Random.Bool());
            
        Projects = projectFaker.Generate(1258);
        lastIndex = Projects.Count - 1;
        currentIndex = 0;

        sb.AppendLine();
        sb.AppendLine("""
                        INSERT INTO "Projects" ("Id", "Name", "OrganizationId", "CreatedAt", "UpdatedAt", "DeletedAt", "Active") VALUES  
                        """);        
        foreach (var item in Projects)
        {
            bool isLast = currentIndex == lastIndex;
            sb.AppendLine($"""
                           ('{item.Id}'::UUID, '{item.Name?.Replace("'", "''")}', '{Organizations[random.Next(Organizations.Count)].Id}'::UUID, '{item.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")}', '{(!item.Active ? item.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") : null)}', {item.Active.ToString().ToLower()}){(isLast ? ";" : ",")}
                           """);
            currentIndex++;
        }

        var impedimentFaker = new Faker<Impediment>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(x => x.Name, f => string.Format("{0} {1} {2}", f.Hacker.Noun(), f.Hacker.IngVerb(), f.Hacker.Adjective()))
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(o => o.DeletedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(f.Random.Number(-7, -6))))
            .RuleFor(x => x.Active, f => f.Random.Bool());
            
        Impediments = impedimentFaker.Generate(114);
        lastIndex = Impediments.Count - 1;
        currentIndex = 0;  

        sb.AppendLine();
        sb.AppendLine("""
                        INSERT INTO "Impediments" ("Id", "Name", "CreatedAt", "UpdatedAt", "DeletedAt", "Active") VALUES  
                        """);        
        foreach (var item in Impediments)
        {
            bool isLast = currentIndex == lastIndex;
            sb.AppendLine($"""
                           ('{item.Id}'::UUID, '{item.Name?.Replace("'", "''")}', '{item.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")}', '{(!item.Active ? item.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") : null)}', {item.Active.ToString().ToLower()}){(isLast ? ";" : ",")}
                           """);
            currentIndex++;
        }

        var assignmentTypeFaker = new Faker<AssignmentType>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(x => x.Name, f => string.Format("{0} {1} {2}", f.Hacker.Noun(), f.Hacker.IngVerb(), f.Hacker.Adjective()))
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(o => o.DeletedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(f.Random.Number(-7, -6))))
            .RuleFor(x => x.Active, f => f.Random.Bool());
            
        AssignmentTypes = assignmentTypeFaker.Generate(3);
        lastIndex = AssignmentTypes.Count - 1;
        currentIndex = 0;  

        sb.AppendLine();
        sb.AppendLine("""
                        INSERT INTO "AssignmentTypes" ("Id", "Name", "CreatedAt", "UpdatedAt", "DeletedAt", "Active") VALUES  
                        """);        
        foreach (var item in AssignmentTypes)
        {
            bool isLast = currentIndex == lastIndex;
            sb.AppendLine($"""
                           ('{item.Id}'::UUID, '{item.Name?.Replace("'", "''")}', '{item.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")}', '{(!item.Active ? item.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") : null)}', {item.Active.ToString().ToLower()}){(isLast ? ";" : ",")}
                           """);
            currentIndex++;
        }

        var workflowFaker = new Faker<Workflow>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(x => x.Name, f => string.Format("{0} {1} {2}", f.Hacker.Noun(), f.Hacker.IngVerb(), f.Hacker.Adjective()))
            .RuleFor(x => x.Order, f => f.IndexGlobal + 1)
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(o => o.DeletedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(f.Random.Number(-7, -6))))
            .RuleFor(x => x.Active, f => f.Random.Bool());
            
        Workflows = workflowFaker.Generate(6);        
        lastIndex = Workflows.Count - 1;
        currentIndex = 0;  

        sb.AppendLine();
        sb.AppendLine("""
                        INSERT INTO "Workflows" ("Id", "Name", "Order", "CreatedAt", "UpdatedAt", "DeletedAt", "Active") VALUES  
                        """);        
        foreach (var item in Workflows)
        {
            bool isLast = currentIndex == lastIndex;
            sb.AppendLine($"""
                           ('{item.Id}'::UUID, '{item.Name?.Replace("'", "''")}', {item.Order}, '{item.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")}', '{(!item.Active ? item.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") : null)}', {item.Active.ToString().ToLower()}){(isLast ? ";" : ",")}
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
            .RuleFor(o => o.DeletedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(f.Random.Number(-7, -6))))
            .RuleFor(x => x.Active, f => f.Random.Bool());
            
        Users = userFaker.Generate(11154);         
        lastIndex = Users.Count - 1;
        currentIndex = 0;  

        sb.AppendLine();
        sb.AppendLine("""
                        INSERT INTO "Users" ("Id", "Name", "Login", "Password", "Salt", "CreatedAt", "UpdatedAt", "DeletedAt", "Active") VALUES  
                        """);        
        foreach (var item in Users)
        {
            bool isLast = currentIndex == lastIndex;
            sb.AppendLine($"""
                           ('{item.Id}'::UUID, '{item.Name?.Replace("'", "''")}', '{item.Login}', '{item.Password}', '{item.Salt}', '{item.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")}', '{(!item.Active ? item.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") : null)}', {item.Active.ToString().ToLower()}){(isLast ? ";" : ",")}
                           """);
            currentIndex++;
        }

        var userProjectFaker = new Faker<UserProject>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(o => o.DeletedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(f.Random.Number(-7, -6))))
            .RuleFor(x => x.Active, f => f.Random.Bool());
            
        UserProjects = userProjectFaker.Generate(24400);
        lastIndex = UserProjects.Count - 1;
        currentIndex = 0;  

        sb.AppendLine();
        sb.AppendLine("""
                        INSERT INTO "UserProjects" ("Id", "UserId", "ProjectId", "CreatedAt", "UpdatedAt", "DeletedAt", "Active") VALUES  
                        """);        
        foreach (var item in UserProjects)
        {
            bool isLast = currentIndex == lastIndex;
            sb.AppendLine($"""
                           ('{item.Id}'::UUID, '{Users[random.Next(Users.Count)].Id}'::UUID, '{Projects[random.Next(Projects.Count)].Id}'::UUID, '{item.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")}', '{(!item.Active ? item.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") : null)}', {item.Active.ToString().ToLower()}){(isLast ? ";" : ",")}
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
            .RuleFor(o => o.DeletedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(f.Random.Number(-7, -6))))
            .RuleFor(x => x.Active, f => f.Random.Bool());

        Assignments = assignmentFaker.Generate(464587);    
        lastIndex = Assignments.Count - 1;
        currentIndex = 0;  

        sb.AppendLine();
        sb.AppendLine("""
                        INSERT INTO "Assignments" ("Id", "Name", "Description", "StartDate", "EndDate", "AmountHours", "ProjectId", "WorkflowId", "UserId", "AssignmentTypeId", "CreatedAt", "UpdatedAt", "DeletedAt", "Active") VALUES  
                        """);        
        foreach (var item in Assignments)
        {
            bool isLast = currentIndex == lastIndex;
            sb.AppendLine($"""
                            ('{item.Id}'::UUID, '{item.Name?.Replace("'", "''")}', '{item.Description?.Replace("'", "''")}', '{item.StartDate.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.EndDate.ToString("yyyy-MM-dd HH:mm:ss")}', {item.AmountHours}, '{Projects[random.Next(Projects.Count)].Id}'::UUID, '{Workflows[random.Next(Workflows.Count)].Id}'::UUID, '{Users[random.Next(Users.Count)].Id}'::UUID, '{AssignmentTypes[random.Next(AssignmentTypes.Count)].Id}'::UUID, '{item.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")}', '{(!item.Active ? item.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") : null)}', {item.Active.ToString().ToLower()}){(isLast ? ";" : ",")}
                            """);
            currentIndex++;
        }

        var userAssignmentFaker = new Faker<UserAssignment>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(o => o.DeletedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(f.Random.Number(-7, -6))))
            .RuleFor(x => x.Active, f => f.Random.Bool());
            
        UserAssignments = userAssignmentFaker.Generate(363554);    
        lastIndex = UserAssignments.Count - 1;
        currentIndex = 0;  

        sb.AppendLine();
        sb.AppendLine("""
                        INSERT INTO "UserAssignments" ("Id", "UserId", "AssignmentId", "CreatedAt", "UpdatedAt", "DeletedAt", "Active") VALUES  
                        """);        
        foreach (var item in UserAssignments)
        {
            bool isLast = currentIndex == lastIndex;
            sb.AppendLine($"""
                           ('{item.Id}'::UUID, '{Users[random.Next(Users.Count)].Id}'::UUID, '{Assignments[random.Next(Assignments.Count)].Id}'::UUID, '{item.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")}', '{(!item.Active ? item.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") : null)}', {item.Active.ToString().ToLower()}){(isLast ? ";" : ",")}
                           """);
            currentIndex++;
        }

        var assignmentImpedimentFaker = new Faker<AssignmentImpediment>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(o => o.DeletedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(f.Random.Number(-7, -6))))
            .RuleFor(x => x.Active, f => f.Random.Bool());
            
        AssignmentImpediments = assignmentImpedimentFaker.Generate(11369);     
        lastIndex = AssignmentImpediments.Count - 1;
        currentIndex = 0;  

        sb.AppendLine();
        sb.AppendLine("""
                        INSERT INTO "AssignmentImpediments" ("Id", "Description", "AssignmentId", "ImpedimentId", "CreatedAt", "UpdatedAt", "DeletedAt", "Active") VALUES  
                        """);        
        foreach (var item in AssignmentImpediments)
        {
            bool isLast = currentIndex == lastIndex;
            sb.AppendLine($"""
                           ('{item.Id}'::UUID, '{item.Description?.Replace("'", "''")}', '{Assignments[random.Next(Assignments.Count)].Id}'::UUID, '{Impediments[random.Next(Impediments.Count)].Id}'::UUID, '{item.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")}', '{(!item.Active ? item.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") : null)}', {item.Active.ToString().ToLower()}){(isLast ? ";" : ",")}
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
            .RuleFor(o => o.DeletedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(f.Random.Number(-7, -6))))
            .RuleFor(x => x.Active, f => f.Random.Bool());

        Appointments = appointmentFaker.Generate(489571);      
        lastIndex = Appointments.Count - 1;
        currentIndex = 0;  

        sb.AppendLine();
        sb.AppendLine("""
                        INSERT INTO "Appointments" ("Id", "Description", "KeepDate", "AmountHours", "AssignmentId", "UserId", "CreatedAt", "UpdatedAt", "DeletedAt", "Active") VALUES  
                        """);        
        foreach (var item in Appointments)
        {
            bool isLast = currentIndex == lastIndex;
            sb.AppendLine($"""
                           ('{item.Id}'::UUID, '{item.Description?.Replace("'", "''")}', '{item.KeepDate.ToString("yyyy-MM-dd HH:mm:ss")}', {item.AmountHours}, '{Assignments[random.Next(Assignments.Count)].Id}'::UUID, '{Users[random.Next(Users.Count)].Id}'::UUID, '{item.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")}', '{(!item.Active ? item.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") : null)}', {item.Active.ToString().ToLower()}){(isLast ? ";" : ",")}
                           """);
            currentIndex++;
        }     

        string filePath = "003-database-dump-dml.sql";
        File.WriteAllText(filePath, sb.ToString());             
    }

    internal static void CreateSqlCsvDumpFile()
    {
        var random = new Random();
        var sb = new StringBuilder();

        Directory.CreateDirectory("dml-data");

        var organizationFaker = new Faker<Organization>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(x => x.Name, f => $"{f.Hacker.Noun()} {f.Hacker.IngVerb()} {f.Hacker.Adjective()}")
            .RuleFor(x => x.Description, f => f.Hacker.Phrase())
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(o => o.DeletedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(f.Random.Number(-7, -6))))
            .RuleFor(x => x.Active, f => f.Random.Bool());

        Organizations = organizationFaker.Generate(686);
        WriteCsv("Organizations.csv", Organizations, org =>
        [
            org.Id.ToString(),
            org.Name!,
            org.Description!,
            org.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            org.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")!,
            !org.Active ? org.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") : null,
            org.Active.ToString().ToLower()
        ]);
        sb.AppendLine("""COPY "Organizations" ("Id", "Name", "Description", "CreatedAt", "UpdatedAt", "DeletedAt", "Active") FROM '/docker-entrypoint-initdb.d/dml-data/Organizations.csv' WITH (FORMAT CSV);""");

        var projectFaker = new Faker<Project>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(x => x.Name, f => $"{f.Hacker.Noun()} {f.Hacker.IngVerb()} {f.Hacker.Adjective()}")
            .RuleFor(p => p.OrganizationId, f => f.PickRandom(Organizations).Id)
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(o => o.DeletedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(f.Random.Number(-7, -6))))
            .RuleFor(x => x.Active, f => f.Random.Bool());

        Projects = projectFaker.Generate(1258);
        WriteCsv("Projects.csv", Projects, p =>
        [
            p.Id.ToString(),
            p.Name!,
            p.OrganizationId.ToString(),
            p.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            p.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")!,
            !p.Active ? p.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") : null,
            p.Active.ToString().ToLower()
        ]);
        sb.AppendLine("""COPY "Projects" ("Id", "Name", "OrganizationId", "CreatedAt", "UpdatedAt", "DeletedAt", "Active") FROM '/docker-entrypoint-initdb.d/dml-data/Projects.csv' WITH (FORMAT CSV);""");

        var impedimentFaker = new Faker<Impediment>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(x => x.Name, f => $"{f.Hacker.Noun()} {f.Hacker.IngVerb()} {f.Hacker.Adjective()}")
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(o => o.DeletedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(f.Random.Number(-7, -6))))
            .RuleFor(x => x.Active, f => f.Random.Bool());

        Impediments = impedimentFaker.Generate(114);
        WriteCsv("Impediments.csv", Impediments, i =>
        [
            i.Id.ToString(),
            i.Name!,
            i.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            i.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")!,
            !i.Active ? i.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") : null,
            i.Active.ToString().ToLower()
        ]);
        sb.AppendLine("""COPY "Impediments" ("Id", "Name", "CreatedAt", "UpdatedAt", "DeletedAt", "Active") FROM '/docker-entrypoint-initdb.d/dml-data/Impediments.csv' WITH (FORMAT CSV);""");

        var assignmentTypeFaker = new Faker<AssignmentType>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(x => x.Name, f => $"{f.Hacker.Noun()} {f.Hacker.IngVerb()} {f.Hacker.Adjective()}")
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(o => o.DeletedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(f.Random.Number(-7, -6))))
            .RuleFor(x => x.Active, f => f.Random.Bool());

        AssignmentTypes = assignmentTypeFaker.Generate(3);
        WriteCsv("AssignmentTypes.csv", AssignmentTypes, at =>
        [
            at.Id.ToString(),
            at.Name!,
            at.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            at.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")!,
            !at.Active ? at.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") : null,
            at.Active.ToString().ToLower()
        ]);
        sb.AppendLine("""COPY "AssignmentTypes" ("Id", "Name", "CreatedAt", "UpdatedAt", "DeletedAt", "Active") FROM '/docker-entrypoint-initdb.d/dml-data/AssignmentTypes.csv' WITH (FORMAT CSV);""");

        var workflowFaker = new Faker<Workflow>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(x => x.Name, f => $"{f.Hacker.Noun()} {f.Hacker.IngVerb()} {f.Hacker.Adjective()}")
            .RuleFor(x => x.Order, f => f.IndexGlobal + 1)
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(o => o.DeletedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(f.Random.Number(-7, -6))))
            .RuleFor(x => x.Active, f => f.Random.Bool());

        Workflows = workflowFaker.Generate(6);
        WriteCsv("Workflows.csv", Workflows, w =>
        [
            w.Id.ToString(),
            w.Name!,
            w.Order.ToString(),
            w.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            w.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")!,
            !w.Active ? w.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") : null,
            w.Active.ToString().ToLower()
        ]);
        sb.AppendLine("""COPY "Workflows" ("Id", "Name", "Order", "CreatedAt", "UpdatedAt", "DeletedAt", "Active") FROM '/docker-entrypoint-initdb.d/dml-data/Workflows.csv' WITH (FORMAT CSV);""");

        var userFaker = new Faker<User>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(x => x.Name, f => f.Name.FullName())
            .RuleFor(x => x.Login, f => f.Internet.UserName())
            .RuleFor(x => x.Password, f => string.Concat(f.Internet.Password(), f.Internet.Password(), f.Internet.Password()))
            .RuleFor(x => x.Salt, f => f.Internet.Password())
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(o => o.DeletedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(f.Random.Number(-7, -6))))
            .RuleFor(x => x.Active, f => f.Random.Bool());

        Users = userFaker.Generate(11154);
        WriteCsv("Users.csv", Users, u =>
        [
            u.Id.ToString(),
            u.Name!,
            u.Login!,
            u.Password!,
            u.Salt!,
            u.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            u.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")!,
            !u.Active ? u.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") : null,
            u.Active.ToString().ToLower()
        ]);
        sb.AppendLine("""COPY "Users" ("Id", "Name", "Login", "Password", "Salt", "CreatedAt", "UpdatedAt", "DeletedAt", "Active") FROM '/docker-entrypoint-initdb.d/dml-data/Users.csv' WITH (FORMAT CSV);""");

        var userProjectFaker = new Faker<UserProject>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(up => up.UserId, f => f.PickRandom(Users).Id)
            .RuleFor(up => up.ProjectId, f => f.PickRandom(Projects).Id)
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(o => o.DeletedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(f.Random.Number(-7, -6))))
            .RuleFor(x => x.Active, f => f.Random.Bool());

        UserProjects = userProjectFaker.Generate(24400);
        WriteCsv("UserProjects.csv", UserProjects, up =>
        [
            up.Id.ToString(),
            up.UserId.ToString(),
            up.ProjectId.ToString(),
            up.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            up.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")!,
            !up.Active ? up.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") : null,
            up.Active.ToString().ToLower()
        ]);
        sb.AppendLine("""COPY "UserProjects" ("Id", "UserId", "ProjectId", "CreatedAt", "UpdatedAt", "DeletedAt", "Active") FROM '/docker-entrypoint-initdb.d/dml-data/UserProjects.csv' WITH (FORMAT CSV);""");

        var assignmentFaker = new Faker<Assignment>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(x => x.Name, f => $"{f.Hacker.Noun()} {f.Hacker.IngVerb()} {f.Hacker.Adjective()}")
            .RuleFor(x => x.Description, f => f.Hacker.Phrase())
            .RuleFor(o => o.StartDate, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.EndDate, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(x => x.AmountHours, f => f.Random.Number(12, 60))
            .RuleFor(a => a.ProjectId, f => f.PickRandom(Projects).Id)
            .RuleFor(a => a.WorkflowId, f => f.PickRandom(Workflows).Id)
            .RuleFor(a => a.UserId, f => f.PickRandom(Users).Id)
            .RuleFor(a => a.AssignmentTypeId, f => f.PickRandom(AssignmentTypes).Id)
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(o => o.DeletedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(f.Random.Number(-7, -6))))
            .RuleFor(x => x.Active, f => f.Random.Bool());

        Assignments = assignmentFaker.Generate(464587);
        WriteCsv("Assignments.csv", Assignments, a =>
        [
            a.Id.ToString(),
            a.Name!,
            a.Description!,
            a.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
            a.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
            a.AmountHours.ToString(),
            a.ProjectId.ToString(),
            a.WorkflowId.ToString(),
            a.UserId.ToString(),
            a.AssignmentTypeId.ToString(),
            a.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            a.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")!,
            !a.Active ? a.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") : null,
            a.Active.ToString().ToLower()
        ]);
        sb.AppendLine("""COPY "Assignments" ("Id", "Name", "Description", "StartDate", "EndDate", "AmountHours", "ProjectId", "WorkflowId", "UserId", "AssignmentTypeId", "CreatedAt", "UpdatedAt", "DeletedAt", "Active") FROM '/docker-entrypoint-initdb.d/dml-data/Assignments.csv' WITH (FORMAT CSV);""");

        var userAssignmentFaker = new Faker<UserAssignment>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(ua => ua.UserId, f => f.PickRandom(Users).Id)
            .RuleFor(ua => ua.AssignmentId, f => f.PickRandom(Assignments).Id)
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(o => o.DeletedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(f.Random.Number(-7, -6))))
            .RuleFor(x => x.Active, f => f.Random.Bool());

        UserAssignments = userAssignmentFaker.Generate(363554);
        WriteCsv("UserAssignments.csv", UserAssignments, ua =>
        [
            ua.Id.ToString(),
            ua.UserId.ToString(),
            ua.AssignmentId.ToString(),
            ua.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            ua.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")!,
            !ua.Active ? ua.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") : null,
            ua.Active.ToString().ToLower()
        ]);
        sb.AppendLine("""COPY "UserAssignments" ("Id", "UserId", "AssignmentId", "CreatedAt", "UpdatedAt", "DeletedAt", "Active") FROM '/docker-entrypoint-initdb.d/dml-data/UserAssignments.csv' WITH (FORMAT CSV);""");

        var assignmentImpedimentFaker = new Faker<AssignmentImpediment>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(ai => ai.AssignmentId, f => f.PickRandom(Assignments).Id)
            .RuleFor(ai => ai.ImpedimentId, f => f.PickRandom(Impediments).Id)
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(o => o.DeletedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(f.Random.Number(-7, -6))))
            .RuleFor(x => x.Active, f => f.Random.Bool());

        AssignmentImpediments = assignmentImpedimentFaker.Generate(11369);
        WriteCsv("AssignmentImpediments.csv", AssignmentImpediments, ai =>
        [
            ai.Id.ToString(),
            ai.Description!,
            ai.AssignmentId.ToString(), 
            ai.ImpedimentId.ToString(),
            ai.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            ai.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")!,
            !ai.Active ? ai.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") : null,
            ai.Active.ToString().ToLower()
        ]);
        sb.AppendLine("""COPY "AssignmentImpediments" ("Id", "Description", "AssignmentId", "ImpedimentId", "CreatedAt", "UpdatedAt", "DeletedAt", "Active") FROM '/docker-entrypoint-initdb.d/dml-data/AssignmentImpediments.csv' WITH (FORMAT CSV);""");

        var appointmentFaker = new Faker<Appointment>()
            .RuleFor(c => c.Id, f => Guid.NewGuid())
            .RuleFor(x => x.Description, f => f.Hacker.Phrase())
            .RuleFor(o => o.KeepDate, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -9)), DateTime.UtcNow.AddMonths(f.Random.Number(-8, -6))))
            .RuleFor(x => x.AmountHours, f => f.Random.Number(1, 6))
            .RuleFor(a => a.AssignmentId, f => f.PickRandom(Assignments).Id)
            .RuleFor(a => a.UserId, f => f.PickRandom(Users).Id)
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(o => o.DeletedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(f.Random.Number(-7, -6))))
            .RuleFor(x => x.Active, f => f.Random.Bool());

        Appointments = appointmentFaker.Generate(489571);
        WriteCsv("Appointments.csv", Appointments, a =>
        [
            a.Id.ToString(),
            a.Description!,
            a.KeepDate.ToString("yyyy-MM-dd HH:mm:ss"),
            a.AmountHours.ToString(),
            a.AssignmentId.ToString(),
            a.UserId.ToString(),
            a.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            a.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")!,
            !a.Active ? a.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") : null,
            a.Active.ToString().ToLower()
        ]);
        sb.AppendLine("""COPY "Appointments" ("Id", "Description", "KeepDate", "AmountHours", "AssignmentId", "UserId", "CreatedAt", "UpdatedAt", "DeletedAt", "Active") FROM '/docker-entrypoint-initdb.d/dml-data/Appointments.csv' WITH (FORMAT CSV);""");

        string filePath = "003-database-dump-csv-dml.sql";
        File.WriteAllText(filePath, sb.ToString());
    }

    private static string EscapeCsvField(string value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        if (value.Contains(",") || value.Contains("\"") || value.Contains("\n") || value.Contains("\r"))
        {
            return $"\"{value.Replace("\"", "\"\"")}\"";
        }
        return value;
    }

    private static void WriteCsv<T>(string fileName, IEnumerable<T> items, Func<T, string[]> getFields)
    {
        string path = Path.Combine("dml-data", fileName);
        using var writer = new StreamWriter(path);
        foreach (var item in items)
        {
            var fields = getFields(item).Select(f => EscapeCsvField(f)).ToArray();
            writer.WriteLine(string.Join(",", fields));
        }
    }
}