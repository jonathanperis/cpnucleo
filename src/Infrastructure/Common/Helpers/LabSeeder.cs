namespace Infrastructure.Common.Helpers;

public static class LabSeeder
{
    public static async Task SeedAsync(ApplicationDbContext database, string profile, string password, CancellationToken cancellationToken = default)
    {
        var projectCount = profile switch
        {
            "tiny" => 3,
            "realistic" => 50,
            _ => throw new ArgumentException("Choose tiny or realistic. Use the explicit bulk importer for the large dataset.", nameof(profile))
        };
        if (await database.Organizations!.IgnoreQueryFilters().AnyAsync(cancellationToken))
            throw new InvalidOperationException("Lab seeding requires an empty database. Reset the disposable lab explicitly first.");

        var organization = Organization.Create("Learning school", "Disposable practice workspace");
        var user = User.Create("Learning administrator", "demo@cpnucleo.local", new Argon2PasswordHasher().Hash(password));
        var workflow = Workflow.Create("Planned", 1);
        var type = AssignmentType.Create("Exercise");
        var impediment = Impediment.Create("Needs investigation");
        database.AddRange(organization, user, workflow, type, impediment);
        var now = DateTime.UtcNow;
        for (var i = 0; i < projectCount; i++)
        {
            var project = Project.Create($"Learning project {i + 1:D3}", organization.Id);
            database.Add(project);
            database.Add(UserProject.Create(user.Id, project.Id));
            for (var j = 0; j < 10; j++)
            {
                var assignment = Assignment.Create($"Exercise {i + 1}-{j + 1}", "Trace, change, test and explain this use case.",
                    now, now.AddDays(7), 2, project.Id, workflow.Id, user.Id, type.Id);
                database.Add(assignment);
                database.Add(UserAssignment.Create(user.Id, assignment.Id));
                database.Add(Appointment.Create("Review the exercise", now.AddDays(1), 1, assignment.Id, user.Id));
            }
        }
        await database.SaveChangesAsync(cancellationToken);
    }
}
