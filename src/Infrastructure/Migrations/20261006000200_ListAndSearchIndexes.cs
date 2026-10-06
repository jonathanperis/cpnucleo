using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations;

/// <summary>
/// Indexes for the soft-delete aware access paths: partial indexes on active rows for listing,
/// counting, relationship checks and membership lookups, plus trigram indexes so the bounded
/// <c>ILIKE '%term%'</c> search doesn't scan whole tables.
/// </summary>
[DbContext(typeof(ApplicationDbContext))]
[Migration("20261006000200_ListAndSearchIndexes")]
public sealed class ListAndSearchIndexes : Migration
{
    private static readonly string[] Tables =
    [
        "Organizations", "Projects", "Workflows", "AssignmentTypes", "Impediments", "Users",
        "Assignments", "Appointments", "AssignmentImpediments", "UserAssignments", "UserProjects"
    ];

    private static readonly (string Table, string Column)[] SearchColumns =
    [
        ("Organizations", "Name"), ("Organizations", "Description"),
        ("Projects", "Name"),
        ("Workflows", "Name"),
        ("AssignmentTypes", "Name"),
        ("Impediments", "Name"),
        ("Users", "Name"), ("Users", "Login"),
        ("Assignments", "Name"), ("Assignments", "Description"),
        ("Appointments", "Description"),
        ("AssignmentImpediments", "Description")
    ];

    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("CREATE EXTENSION IF NOT EXISTS pg_trgm;");

        foreach (var table in Tables)
            migrationBuilder.Sql($"""CREATE INDEX "IX_{table}_Active_Id" ON "{table}" ("Id") WHERE "Active";""");

        foreach (var (table, column, _) in RelationshipIntegrity.References)
            migrationBuilder.Sql($"""CREATE INDEX "IX_{table}_{column}_Active" ON "{table}" ("{column}") WHERE "Active";""");

        migrationBuilder.Sql("""CREATE INDEX "IX_UserProjects_Membership" ON "UserProjects" ("UserId", "ProjectId") WHERE "Active";""");

        foreach (var (table, column) in SearchColumns)
            migrationBuilder.Sql($"""CREATE INDEX "IX_{table}_{column}_Trgm" ON "{table}" USING gin ("{column}" gin_trgm_ops) WHERE "Active";""");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        foreach (var (table, column) in SearchColumns)
            migrationBuilder.Sql($"""DROP INDEX "IX_{table}_{column}_Trgm";""");

        migrationBuilder.Sql("""DROP INDEX "IX_UserProjects_Membership";""");

        foreach (var (table, column, _) in RelationshipIntegrity.References)
            migrationBuilder.Sql($"""DROP INDEX "IX_{table}_{column}_Active";""");

        foreach (var table in Tables)
            migrationBuilder.Sql($"""DROP INDEX "IX_{table}_Active_Id";""");

        // pg_trgm is left installed: other objects may depend on it.
    }
}
