using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations;

/// <summary>
/// Applies <see cref="DemoWorkspaceNames"/> once more now that it also recognises Bogus Hacker names
/// with words appended: production's report listed one project, "feed indexing redundant x".
/// </summary>
[DbContext(typeof(ApplicationDbContext))]
[Migration("20261009000300_DemoWorkspaceSuffixedNames")]
public sealed class DemoWorkspaceSuffixedNames : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder) => DemoWorkspaceNamesData.Apply(migrationBuilder);

    // The replaced Bogus text has no value and is not kept, so there is nothing to restore.
    protected override void Down(MigrationBuilder migrationBuilder)
    {
    }
}
