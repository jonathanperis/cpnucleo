namespace Infrastructure.Common.Helpers;

internal static class FakeDataHelper
{
    private const string DefaultDemoLogin = "demo@cpnucleo.local";
    private const string DefaultDemoPassword = "CpnucleoDemo2026!";
    private const string DefaultDemoName = "Cpnucleo Demo";
    private static readonly Guid DefaultDemoUserId = Guid.Parse("0198a4a8-6d1f-7a54-9b1c-c9c430f2d001");

    private static List<Appointment>? Appointments { get; set; }
    private static List<AssignmentImpediment>? AssignmentImpediments { get; set; }
    private static List<Assignment>? Assignments { get; set; }
    private static List<AssignmentType>? AssignmentTypes { get; set; }
    private static List<Impediment>? Impediments { get; set; }
    private static List<Organization>? Organizations { get; set; }
    private static List<Project>? Projects { get; set; }
    private static List<UserAssignment>? UserAssignments { get; set; }
    private static List<User>? Users { get; set; }
    private static List<UserProject>? UserProjects { get; set; }
    private static List<Workflow>? Workflows { get; set; }
    
    internal static void CreateSqlDumpFile()
    {
        var random = new Random();
        var sb = new StringBuilder();
        var fakeUserPasswordHash = new Argon2PasswordHasher().Hash("FakeUser@123");
        var defaultDemoPasswordHash = new Argon2PasswordHasher().Hash(DefaultDemoPassword);
        AppendDatabaseResetAndDefaultUserSql(sb, defaultDemoPasswordHash);
        
        var organizationFaker = new Faker<Organization>()
            .RuleFor(c => c.Id, f => BaseEntity.GetNewId())
            .RuleFor(x => x.Name, f => $"{f.Hacker.Noun()} {f.Hacker.IngVerb()} {f.Hacker.Adjective()}")
            .RuleFor(x => x.Description, f => f.Hacker.Phrase())
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(o => o.DeletedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(f.Random.Number(-7, -6))))
            .RuleFor(o => o.DeletedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(f.Random.Number(-7, -6))))
            .RuleFor(x => x.Active, f => f.Random.Bool());

        Organizations = organizationFaker.Generate(686);
        var lastIndex = Organizations.Count - 1;
        var currentIndex = 0;
        
        sb.AppendLine("""
                        INSERT INTO "Organizations" ("Id", "Name", "Description", "CreatedAt", "UpdatedAt", "DeletedAt", "Active") VALUES 
                        """);

        foreach (var item in Organizations)
        {
            var isLast = currentIndex == lastIndex;
            sb.AppendLine($"('{item.Id}'::UUID, '{item.Name?.Replace("'", "''")}', '{item.Description?.Replace("'", "''")}', '{item.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")}', '{(!item.Active ? item.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") : null)}', {item.Active.ToString().ToLower()}){(isLast ? ";" : ",")}");
            currentIndex++;
        }

        var projectFaker = new Faker<Project>()
            .RuleFor(c => c.Id, f => BaseEntity.GetNewId())
            .RuleFor(x => x.Name, f => $"{f.Hacker.Noun()} {f.Hacker.IngVerb()} {f.Hacker.Adjective()}")
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
            var isLast = currentIndex == lastIndex;
            sb.AppendLine($"('{item.Id}'::UUID, '{item.Name?.Replace("'", "''")}', '{Organizations[random.Next(Organizations.Count)].Id}'::UUID, '{item.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")}', '{(!item.Active ? item.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") : null)}', {item.Active.ToString().ToLower()}){(isLast ? ";" : ",")}");
            currentIndex++;
        }

        var impedimentFaker = new Faker<Impediment>()
            .RuleFor(c => c.Id, f => BaseEntity.GetNewId())
            .RuleFor(x => x.Name, f => $"{f.Hacker.Noun()} {f.Hacker.IngVerb()} {f.Hacker.Adjective()}")
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
            var isLast = currentIndex == lastIndex;
            sb.AppendLine($"('{item.Id}'::UUID, '{item.Name?.Replace("'", "''")}', '{item.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")}', '{(!item.Active ? item.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") : null)}', {item.Active.ToString().ToLower()}){(isLast ? ";" : ",")}");
            currentIndex++;
        }

        var assignmentTypeFaker = new Faker<AssignmentType>()
            .RuleFor(c => c.Id, f => BaseEntity.GetNewId())
            .RuleFor(x => x.Name, f => $"{f.Hacker.Noun()} {f.Hacker.IngVerb()} {f.Hacker.Adjective()}")
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
            var isLast = currentIndex == lastIndex;
            sb.AppendLine($"('{item.Id}'::UUID, '{item.Name?.Replace("'", "''")}', '{item.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")}', '{(!item.Active ? item.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") : null)}', {item.Active.ToString().ToLower()}){(isLast ? ";" : ",")}");
            currentIndex++;
        }

        var workflowFaker = new Faker<Workflow>()
            .RuleFor(c => c.Id, f => BaseEntity.GetNewId())
            .RuleFor(x => x.Name, f => $"{f.Hacker.Noun()} {f.Hacker.IngVerb()} {f.Hacker.Adjective()}")
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
            var isLast = currentIndex == lastIndex;
            sb.AppendLine($"('{item.Id}'::UUID, '{item.Name?.Replace("'", "''")}', {item.Order}, '{item.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")}', '{(!item.Active ? item.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") : null)}', {item.Active.ToString().ToLower()}){(isLast ? ";" : ",")}");
            currentIndex++;
        }

        var userFaker = new Faker<User>()
            .RuleFor(c => c.Id, f => BaseEntity.GetNewId())
            .RuleFor(x => x.Name, f => f.Name.FullName())
            .RuleFor(x => x.Login, f => f.Internet.UserName())
            .RuleFor(x => x.Password, _ => fakeUserPasswordHash.Hash)
            .RuleFor(x => x.Salt, _ => fakeUserPasswordHash.Salt)
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
            var isLast = currentIndex == lastIndex;
            sb.AppendLine($"('{item.Id}'::UUID, '{item.Name?.Replace("'", "''")}', '{item.Login}', '{item.Password}', '{item.Salt}', '{item.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")}', '{(!item.Active ? item.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") : null)}', {item.Active.ToString().ToLower()}){(isLast ? ";" : ",")}");
            currentIndex++;
        }

        var userProjectFaker = new Faker<UserProject>()
            .RuleFor(c => c.Id, f => BaseEntity.GetNewId())
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
            var isLast = currentIndex == lastIndex;
            sb.AppendLine($"('{item.Id}'::UUID, '{Users[random.Next(Users.Count)].Id}'::UUID, '{Projects[random.Next(Projects.Count)].Id}'::UUID, '{item.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")}', '{(!item.Active ? item.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") : null)}', {item.Active.ToString().ToLower()}){(isLast ? ";" : ",")}");
            currentIndex++;
        }

        var assignmentFaker = new Faker<Assignment>()
            .RuleFor(c => c.Id, f => BaseEntity.GetNewId())
            .RuleFor(x => x.Name, f => $"{f.Hacker.Noun()} {f.Hacker.IngVerb()} {f.Hacker.Adjective()}")
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
            var isLast = currentIndex == lastIndex;
            sb.AppendLine($"('{item.Id}'::UUID, '{item.Name?.Replace("'", "''")}', '{item.Description?.Replace("'", "''")}', '{item.StartDate.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.EndDate.ToString("yyyy-MM-dd HH:mm:ss")}', {item.AmountHours}, '{Projects[random.Next(Projects.Count)].Id}'::UUID, '{Workflows[random.Next(Workflows.Count)].Id}'::UUID, '{Users[random.Next(Users.Count)].Id}'::UUID, '{AssignmentTypes[random.Next(AssignmentTypes.Count)].Id}'::UUID, '{item.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")}', '{(!item.Active ? item.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") : null)}', {item.Active.ToString().ToLower()}){(isLast ? ";" : ",")}");
            currentIndex++;
        }

        var userAssignmentFaker = new Faker<UserAssignment>()
            .RuleFor(c => c.Id, f => BaseEntity.GetNewId())
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
            var isLast = currentIndex == lastIndex;
            sb.AppendLine($"('{item.Id}'::UUID, '{Users[random.Next(Users.Count)].Id}'::UUID, '{Assignments[random.Next(Assignments.Count)].Id}'::UUID, '{item.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")}', '{(!item.Active ? item.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") : null)}', {item.Active.ToString().ToLower()}){(isLast ? ";" : ",")}");
            currentIndex++;
        }

        var assignmentImpedimentFaker = new Faker<AssignmentImpediment>()
            .RuleFor(c => c.Id, f => BaseEntity.GetNewId())
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
            var isLast = currentIndex == lastIndex;
            sb.AppendLine($"('{item.Id}'::UUID, '{item.Description?.Replace("'", "''")}', '{Assignments[random.Next(Assignments.Count)].Id}'::UUID, '{Impediments[random.Next(Impediments.Count)].Id}'::UUID, '{item.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")}', '{(!item.Active ? item.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") : null)}', {item.Active.ToString().ToLower()}){(isLast ? ";" : ",")}");
            currentIndex++;
        }

        var appointmentFaker = new Faker<Appointment>()
            .RuleFor(c => c.Id, f => BaseEntity.GetNewId())
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
            var isLast = currentIndex == lastIndex;
            sb.AppendLine($"('{item.Id}'::UUID, '{item.Description?.Replace("'", "''")}', '{item.KeepDate.ToString("yyyy-MM-dd HH:mm:ss")}', {item.AmountHours}, '{Assignments[random.Next(Assignments.Count)].Id}'::UUID, '{Users[random.Next(Users.Count)].Id}'::UUID, '{item.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}', '{item.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss")}', '{(!item.Active ? item.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") : null)}', {item.Active.ToString().ToLower()}){(isLast ? ";" : ",")}");
            currentIndex++;
        }     

        const string filePath = "003-database-dump-dml.sql";
        File.WriteAllText(filePath, sb.ToString());             
    }

    internal static void CreateSqlCsvDumpFile()
    {
        var sb = new StringBuilder();
        var random = new Random();
        var fakeUserPasswordHash = new Argon2PasswordHasher().Hash("FakeUser@123");
        var defaultDemoPasswordHash = new Argon2PasswordHasher().Hash(DefaultDemoPassword);

        Directory.CreateDirectory("dml-data");
        AppendDatabaseResetAndDefaultUserSql(sb, defaultDemoPasswordHash);

        var organizationFaker = new Faker<Organization>()
            .RuleFor(c => c.Id, f => BaseEntity.GetNewId())
            .RuleFor(x => x.Name, f => $"{f.Hacker.Noun()} {f.Hacker.IngVerb()} {f.Hacker.Adjective()}")
            .RuleFor(x => x.Description, f => f.Hacker.Phrase())
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(o => o.DeletedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(f.Random.Number(-7, -6))))
            .RuleFor(x => x.Active, f => f.Random.Bool());

        Organizations = organizationFaker.Generate(686);
        WriteCsv("Organizations.csv", Organizations, x =>
        [
            x.Id.ToString(),
            x.Name!,
            x.Description!,
            x.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            x.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
            !x.Active ? x.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "" : "",
            x.Active.ToString().ToLower()
        ]);

        sb.AppendLine("""COPY "Organizations" ("Id", "Name", "Description", "CreatedAt", "UpdatedAt", "DeletedAt", "Active") FROM '/docker-entrypoint-initdb.d/dml-data/Organizations.csv' WITH (FORMAT CSV);""");

        var projectFaker = new Faker<Project>()
            .RuleFor(c => c.Id, f => BaseEntity.GetNewId())
            .RuleFor(x => x.Name, f => $"{f.Hacker.Noun()} {f.Hacker.IngVerb()} {f.Hacker.Adjective()}")
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(o => o.DeletedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(f.Random.Number(-7, -6))))
            .RuleFor(x => x.Active, f => f.Random.Bool());

        Projects = projectFaker.Generate(1258);
        for (var i = 0; i < Projects.Count; i++)
        {
            Projects[i].OrganizationId = i < Organizations.Count
                ? Organizations[i].Id
                : PickRandom(Organizations, random).Id;
        }

        WriteCsv("Projects.csv", Projects, x =>
        [
            x.Id.ToString(),
            x.Name!,
            x.OrganizationId.ToString(),
            x.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            x.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
            !x.Active ? x.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "" : "",
            x.Active.ToString().ToLower()
        ]);

        sb.AppendLine("""COPY "Projects" ("Id", "Name", "OrganizationId", "CreatedAt", "UpdatedAt", "DeletedAt", "Active") FROM '/docker-entrypoint-initdb.d/dml-data/Projects.csv' WITH (FORMAT CSV);""");

        var impedimentFaker = new Faker<Impediment>()
            .RuleFor(c => c.Id, f => BaseEntity.GetNewId())
            .RuleFor(x => x.Name, f => $"{f.Hacker.Noun()} {f.Hacker.IngVerb()} {f.Hacker.Adjective()}")
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(o => o.DeletedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(f.Random.Number(-7, -6))))
            .RuleFor(x => x.Active, f => f.Random.Bool());

        Impediments = impedimentFaker.Generate(114);
        WriteCsv("Impediments.csv", Impediments, x =>
        [
            x.Id.ToString(),
            x.Name!,
            x.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            x.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
            !x.Active ? x.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "" : "",
            x.Active.ToString().ToLower()
        ]);

        sb.AppendLine("""COPY "Impediments" ("Id", "Name", "CreatedAt", "UpdatedAt", "DeletedAt", "Active") FROM '/docker-entrypoint-initdb.d/dml-data/Impediments.csv' WITH (FORMAT CSV);""");

        var assignmentTypeFaker = new Faker<AssignmentType>()
            .RuleFor(c => c.Id, f => BaseEntity.GetNewId())
            .RuleFor(x => x.Name, f => $"{f.Hacker.Noun()} {f.Hacker.IngVerb()} {f.Hacker.Adjective()}")
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(o => o.DeletedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(f.Random.Number(-7, -6))))
            .RuleFor(x => x.Active, f => f.Random.Bool());
            
        AssignmentTypes = assignmentTypeFaker.Generate(3);
        WriteCsv("AssignmentTypes.csv", AssignmentTypes, x =>
        [
            x.Id.ToString(),
            x.Name!,
            x.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            x.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
            !x.Active ? x.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "" : "",
            x.Active.ToString().ToLower()
        ]);

        sb.AppendLine("""COPY "AssignmentTypes" ("Id", "Name", "CreatedAt", "UpdatedAt", "DeletedAt", "Active") FROM '/docker-entrypoint-initdb.d/dml-data/AssignmentTypes.csv' WITH (FORMAT CSV);""");

        var workflowFaker = new Faker<Workflow>()
            .RuleFor(c => c.Id, f => BaseEntity.GetNewId())
            .RuleFor(x => x.Name, f => $"{f.Hacker.Noun()} {f.Hacker.IngVerb()} {f.Hacker.Adjective()}")
            .RuleFor(x => x.Order, f => f.IndexGlobal + 1)
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(o => o.DeletedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(f.Random.Number(-7, -6))))
            .RuleFor(x => x.Active, f => f.Random.Bool());
            
        Workflows = workflowFaker.Generate(6);
        WriteCsv("Workflows.csv", Workflows, x =>
        [
            x.Id.ToString(),
            x.Name!,
            x.Order.ToString(),
            x.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            x.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
            !x.Active ? x.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "" : "",
            x.Active.ToString().ToLower()
        ]);

        sb.AppendLine("""COPY "Workflows" ("Id", "Name", "Order", "CreatedAt", "UpdatedAt", "DeletedAt", "Active") FROM '/docker-entrypoint-initdb.d/dml-data/Workflows.csv' WITH (FORMAT CSV);""");

        var userFaker = new Faker<User>()
            .RuleFor(c => c.Id, f => BaseEntity.GetNewId())
            .RuleFor(x => x.Name, f => f.Name.FullName())
            .RuleFor(x => x.Login, f => f.Internet.UserName())
            .RuleFor(x => x.Password, _ => fakeUserPasswordHash.Hash)
            .RuleFor(x => x.Salt, _ => fakeUserPasswordHash.Salt)
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(o => o.DeletedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(f.Random.Number(-7, -6))))
            .RuleFor(x => x.Active, f => f.Random.Bool());

        Users = userFaker.Generate(11154);
        var userOrganizationIds = Users
            .Select((user, index) => new { user.Id, OrganizationId = Organizations[index % Organizations.Count].Id })
            .ToDictionary(x => x.Id, x => x.OrganizationId);

        WriteCsv("Users.csv", Users, x =>
        [
            x.Id.ToString(),
            x.Name!,
            x.Login!,
            x.Password!,
            x.Salt!,
            x.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            x.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
            !x.Active ? x.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "" : "",
            x.Active.ToString().ToLower()
        ]);

        sb.AppendLine("""COPY "Users" ("Id", "Name", "Login", "Password", "Salt", "CreatedAt", "UpdatedAt", "DeletedAt", "Active") FROM '/docker-entrypoint-initdb.d/dml-data/Users.csv' WITH (FORMAT CSV);""");

        UserProjects = Projects
            .Select(project => UserProject.Create(DefaultDemoUserId, project.Id))
            .ToList();

        while (UserProjects.Count < 24400)
        {
            var user = PickRandom(Users, random);
            var project = PickProjectForOrganization(Projects, userOrganizationIds[user.Id], random);
            UserProjects.Add(UserProject.Create(user.Id, project.Id));
        }
        WriteCsv("UserProjects.csv", UserProjects, x =>
        [
            x.Id.ToString(),
            x.UserId.ToString(),
            x.ProjectId.ToString(),
            x.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            x.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
            !x.Active ? x.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "" : "",
            x.Active.ToString().ToLower()
        ]);

        sb.AppendLine("""COPY "UserProjects" ("Id", "UserId", "ProjectId", "CreatedAt", "UpdatedAt", "DeletedAt", "Active") FROM '/docker-entrypoint-initdb.d/dml-data/UserProjects.csv' WITH (FORMAT CSV);""");

        var assignmentFaker = new Faker<Assignment>()
            .RuleFor(c => c.Id, f => BaseEntity.GetNewId())
            .RuleFor(x => x.Name, f => $"{f.Hacker.Noun()} {f.Hacker.IngVerb()} {f.Hacker.Adjective()}")
            .RuleFor(x => x.Description, f => f.Hacker.Phrase())
            .RuleFor(o => o.StartDate, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.EndDate, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(x => x.AmountHours, f => f.Random.Number(12, 60))
            .RuleFor(a => a.WorkflowId, f => f.PickRandom(Workflows).Id)
            .RuleFor(a => a.AssignmentTypeId, f => f.PickRandom(AssignmentTypes).Id)
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(o => o.DeletedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(f.Random.Number(-7, -6))))
            .RuleFor(x => x.Active, f => f.Random.Bool());

        Assignments = assignmentFaker.Generate(464587);
        var assignmentOrganizationIds = new Dictionary<Guid, Guid>(Assignments.Count);
        foreach (var assignment in Assignments)
        {
            var project = PickRandom(Projects, random);
            var organizationId = project.OrganizationId;
            assignment.ProjectId = project.Id;
            assignment.UserId = PickUserForOrganization(Users, userOrganizationIds, organizationId, random).Id;
            assignmentOrganizationIds[assignment.Id] = organizationId;
        }

        WriteCsv("Assignments.csv", Assignments, x =>
        [
            x.Id.ToString(),
            x.Name!,
            x.Description!,
            x.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
            x.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
            x.AmountHours.ToString(),
            x.ProjectId.ToString(),
            x.WorkflowId.ToString(),
            x.UserId.ToString(),
            x.AssignmentTypeId.ToString(),
            x.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            x.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
            !x.Active ? x.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "" : "",
            x.Active.ToString().ToLower()
        ]);

        sb.AppendLine("""COPY "Assignments" ("Id", "Name", "Description", "StartDate", "EndDate", "AmountHours", "ProjectId", "WorkflowId", "UserId", "AssignmentTypeId", "CreatedAt", "UpdatedAt", "DeletedAt", "Active") FROM '/docker-entrypoint-initdb.d/dml-data/Assignments.csv' WITH (FORMAT CSV);""");

        var userAssignmentFaker = new Faker<UserAssignment>()
            .RuleFor(c => c.Id, f => BaseEntity.GetNewId())
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(o => o.DeletedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(f.Random.Number(-7, -6))))
            .RuleFor(x => x.Active, f => f.Random.Bool());

        UserAssignments = userAssignmentFaker.Generate(363554);
        foreach (var userAssignment in UserAssignments)
        {
            var assignment = PickRandom(Assignments, random);
            userAssignment.AssignmentId = assignment.Id;
            userAssignment.UserId = PickUserForOrganization(Users, userOrganizationIds, assignmentOrganizationIds[assignment.Id], random).Id;
        }

        WriteCsv("UserAssignments.csv", UserAssignments, x =>
        [
            x.Id.ToString(),
            x.UserId.ToString(),
            x.AssignmentId.ToString(),
            x.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            x.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
            !x.Active ? x.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "" : "",
            x.Active.ToString().ToLower()
        ]);

        sb.AppendLine("""COPY "UserAssignments" ("Id", "UserId", "AssignmentId", "CreatedAt", "UpdatedAt", "DeletedAt", "Active") FROM '/docker-entrypoint-initdb.d/dml-data/UserAssignments.csv' WITH (FORMAT CSV);""");

        var assignmentImpedimentFaker = new Faker<AssignmentImpediment>()
            .RuleFor(c => c.Id, f => BaseEntity.GetNewId())
            .RuleFor(ai => ai.AssignmentId, f => f.PickRandom(Assignments).Id)
            .RuleFor(ai => ai.ImpedimentId, f => f.PickRandom(Impediments).Id)
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(o => o.DeletedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(f.Random.Number(-7, -6))))
            .RuleFor(x => x.Active, f => f.Random.Bool());

        AssignmentImpediments = assignmentImpedimentFaker.Generate(11369);
        WriteCsv("AssignmentImpediments.csv", AssignmentImpediments, x =>
        [
            x.Id.ToString(),
            x.Description!,
            x.AssignmentId.ToString(), 
            x.ImpedimentId.ToString(),
            x.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            x.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
            !x.Active ? x.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "" : "",
            x.Active.ToString().ToLower()
        ]);

        sb.AppendLine("""COPY "AssignmentImpediments" ("Id", "Description", "AssignmentId", "ImpedimentId", "CreatedAt", "UpdatedAt", "DeletedAt", "Active") FROM '/docker-entrypoint-initdb.d/dml-data/AssignmentImpediments.csv' WITH (FORMAT CSV);""");

        var appointmentFaker = new Faker<Appointment>()
            .RuleFor(c => c.Id, f => BaseEntity.GetNewId())
            .RuleFor(x => x.Description, f => f.Hacker.Phrase())
            .RuleFor(o => o.KeepDate, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -9)), DateTime.UtcNow.AddMonths(f.Random.Number(-8, -6))))
            .RuleFor(x => x.AmountHours, f => f.Random.Number(1, 6))
            .RuleFor(o => o.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-36, -24)), DateTime.UtcNow.AddMonths(f.Random.Number(-24, -12))))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-12, -8)), DateTime.UtcNow.AddMonths(f.Random.Number(-6, -2))))
            .RuleFor(o => o.DeletedAt, f => f.Date.Between(DateTime.UtcNow.AddMonths(f.Random.Number(-11, -7)), DateTime.UtcNow.AddMonths(f.Random.Number(-7, -6))))
            .RuleFor(x => x.Active, f => f.Random.Bool());

        Appointments = appointmentFaker.Generate(489571);
        foreach (var appointment in Appointments)
        {
            var assignment = PickRandom(Assignments, random);
            appointment.AssignmentId = assignment.Id;
            appointment.UserId = PickUserForOrganization(Users, userOrganizationIds, assignmentOrganizationIds[assignment.Id], random).Id;
        }

        WriteCsv("Appointments.csv", Appointments, x =>
        [
            x.Id.ToString(),
            x.Description!,
            x.KeepDate.ToString("yyyy-MM-dd HH:mm:ss"),
            x.AmountHours.ToString(),
            x.AssignmentId.ToString(),
            x.UserId.ToString(),
            x.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            x.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
            !x.Active ? x.DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "" : "",
            x.Active.ToString().ToLower()
        ]);

        sb.AppendLine("""COPY "Appointments" ("Id", "Description", "KeepDate", "AmountHours", "AssignmentId", "UserId", "CreatedAt", "UpdatedAt", "DeletedAt", "Active") FROM '/docker-entrypoint-initdb.d/dml-data/Appointments.csv' WITH (FORMAT CSV);""");

        const string filePath = "003-database-dump-csv-dml.sql";
        File.WriteAllText(filePath, sb.ToString());
    }

    private static void AppendDatabaseResetAndDefaultUserSql(StringBuilder sb, PasswordHash defaultDemoPasswordHash)
    {
        sb.AppendLine($"""
                      TRUNCATE TABLE
                          "Appointments",
                          "AssignmentImpediments",
                          "UserAssignments",
                          "Assignments",
                          "UserProjects",
                          "Projects",
                          "Organizations",
                          "Workflows",
                          "AssignmentTypes",
                          "Impediments"
                      RESTART IDENTITY CASCADE;

                      DELETE FROM "Users" WHERE "Login" <> '{DefaultDemoLogin}';

                      INSERT INTO "Users" ("Id", "Name", "Login", "Password", "Salt", "CreatedAt", "UpdatedAt", "DeletedAt", "Active")
                      SELECT '{DefaultDemoUserId}'::UUID, '{EscapeSqlField(DefaultDemoName)}', '{DefaultDemoLogin}', '{EscapeSqlField(defaultDemoPasswordHash.Hash)}', '{EscapeSqlField(defaultDemoPasswordHash.Salt)}', NOW(), NULL, NULL, true
                      WHERE NOT EXISTS (SELECT 1 FROM "Users" WHERE "Login" = '{DefaultDemoLogin}');

                      UPDATE "Users"
                      SET "Name" = '{EscapeSqlField(DefaultDemoName)}',
                          "Password" = '{EscapeSqlField(defaultDemoPasswordHash.Hash)}',
                          "Salt" = '{EscapeSqlField(defaultDemoPasswordHash.Salt)}',
                          "DeletedAt" = NULL,
                          "Active" = true
                      WHERE "Login" = '{DefaultDemoLogin}';
                      """);
    }

    private static string EscapeSqlField(string value) => value.Replace("'", "''");

    private static T PickRandom<T>(IReadOnlyList<T> values, Random random) => values[random.Next(values.Count)];

    private static Project PickProjectForOrganization(IReadOnlyList<Project> projects, Guid organizationId, Random random)
    {
        var start = random.Next(projects.Count);
        for (var offset = 0; offset < projects.Count; offset++)
        {
            var index = (start + offset) % projects.Count;
            if (projects[index].OrganizationId == organizationId)
            {
                return projects[index];
            }
        }

        throw new InvalidOperationException($"No project exists for organization {organizationId}.");
    }

    private static User PickUserForOrganization(IReadOnlyList<User> users, IReadOnlyDictionary<Guid, Guid> userOrganizationIds, Guid organizationId, Random random)
    {
        var start = random.Next(users.Count);
        for (var offset = 0; offset < users.Count; offset++)
        {
            var index = (start + offset) % users.Count;
            if (userOrganizationIds[users[index].Id] == organizationId)
            {
                return users[index];
            }
        }

        throw new InvalidOperationException($"No user exists for organization {organizationId}.");
    }

    private static string EscapeCsvField(string value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        if (value.Contains(',') || value.Contains('"') || value.Contains('\n') || value.Contains('\r'))
        {
            return $"\"{value.Replace("\"", "\"\"")}\"";
        }
        return value;
    }

    private static void WriteCsv<T>(string fileName, IEnumerable<T> items, Func<T, string[]> getFields)
    {
        // Use Path.GetFileName to ensure fileName is never a rooted path
        var safeFileName = Path.GetFileName(fileName);
        var path = Path.Combine("dml-data", safeFileName);
        using var writer = new StreamWriter(path);
        var lines = items.Select(item => string.Join(",", getFields(item).Select(EscapeCsvField)));
        foreach (var line in lines)
        {
            writer.WriteLine(line);
        }
    }
}