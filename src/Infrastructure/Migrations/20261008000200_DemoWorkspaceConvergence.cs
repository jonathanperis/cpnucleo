using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations;

/// <summary>
/// Runs the newer <see cref="DemoWorkspaceNames"/> script once more on databases the first data
/// migration already renamed. That version recognises its own output, so it also schedules generated
/// tasks onto hand-named board columns, words tasks after their type's name, and renames fake users
/// whose logins came from Bogus user names (they share the importer's one password hash). Rows it
/// already wrote identically are left alone.
/// </summary>
[DbContext(typeof(ApplicationDbContext))]
[Migration("20261008000200_DemoWorkspaceConvergence")]
public sealed class DemoWorkspaceConvergence : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder) => DemoWorkspaceNamesData.Apply(migrationBuilder);

    // Generated text and schedules have no earlier state worth restoring.
    protected override void Down(MigrationBuilder migrationBuilder)
    {
    }
}
