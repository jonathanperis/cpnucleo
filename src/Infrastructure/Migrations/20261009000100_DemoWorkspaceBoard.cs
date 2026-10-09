using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations;

/// <summary>
/// Re-places the already scheduled generated tasks once with the corrected board mapping.
/// <see cref="DemoWorkspaceConvergence"/> sent finished tasks to the last active column, which on a
/// board ending in a Blocked column is not Done. Normal runs of <see cref="DemoWorkspaceNames"/> only
/// place tasks they schedule, so later moves on the board are kept; this migration sets the
/// transaction-local <c>cpnucleo.demo_realign_board</c> flag for this one run. It also applies the
/// newer wording rules (a type named Task gets feature wording).
/// </summary>
[DbContext(typeof(ApplicationDbContext))]
[Migration("20261009000100_DemoWorkspaceBoard")]
public sealed class DemoWorkspaceBoard : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("SET LOCAL cpnucleo.demo_realign_board = 'on';");
        DemoWorkspaceNamesData.Apply(migrationBuilder);
        migrationBuilder.Sql("SET LOCAL cpnucleo.demo_realign_board = 'off';");
    }

    // Board placement of generated tasks has no earlier state worth restoring.
    protected override void Down(MigrationBuilder migrationBuilder)
    {
    }
}
