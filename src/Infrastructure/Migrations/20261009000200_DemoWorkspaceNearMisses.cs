using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations;

/// <summary>
/// Applies <see cref="DemoWorkspaceNames"/> once more now that it also recognises Bogus Hacker names
/// in any letter case and spacing ("Monitor  Transmitting Back-End"): production kept one such
/// project after the earlier data migrations.
/// </summary>
[DbContext(typeof(ApplicationDbContext))]
[Migration("20261009000200_DemoWorkspaceNearMisses")]
public sealed class DemoWorkspaceNearMisses : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder) => DemoWorkspaceNamesData.Apply(migrationBuilder);

    // The replaced Bogus text has no value and is not kept, so there is nothing to restore.
    protected override void Down(MigrationBuilder migrationBuilder)
    {
    }
}
