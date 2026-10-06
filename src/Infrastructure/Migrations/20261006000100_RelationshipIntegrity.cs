using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations;

/// <summary>
/// Soft-delete aware relationship integrity, enforced in PostgreSQL so EF Core, Dapper, REST and
/// gRPC all share one rule set:
/// <list type="bullet">
/// <item>An active row may only reference active parents (SQLSTATE 23503).</item>
/// <item>A parent with active dependent data can't be soft-deleted (SQLSTATE 23001).</item>
/// <item>Membership link rows (UserProjects, UserAssignments) are soft-deleted with their parent.</item>
/// </list>
/// Child checks lock the parent row <c>FOR SHARE</c> and deactivation runs under the parent's row
/// lock, so a concurrent insert and removal can't both succeed. Existing legacy rows are left as
/// they are; they are only re-checked when their references change or they are reactivated.
/// </summary>
[DbContext(typeof(ApplicationDbContext))]
[Migration("20261006000100_RelationshipIntegrity")]
public sealed class RelationshipIntegrity : Migration
{
    /// <summary>Every soft-delete aware reference: (child table, column, parent table).</summary>
    public static readonly (string Child, string Column, string Parent)[] References =
    [
        ("Projects", "OrganizationId", "Organizations"),
        ("Assignments", "ProjectId", "Projects"),
        ("Assignments", "WorkflowId", "Workflows"),
        ("Assignments", "UserId", "Users"),
        ("Assignments", "AssignmentTypeId", "AssignmentTypes"),
        ("Appointments", "AssignmentId", "Assignments"),
        ("Appointments", "UserId", "Users"),
        ("AssignmentImpediments", "AssignmentId", "Assignments"),
        ("AssignmentImpediments", "ImpedimentId", "Impediments"),
        ("UserAssignments", "UserId", "Users"),
        ("UserAssignments", "AssignmentId", "Assignments"),
        ("UserProjects", "UserId", "Users"),
        ("UserProjects", "ProjectId", "Projects")
    ];

    /// <summary>Link tables whose rows follow their parent's removal instead of blocking it.</summary>
    public static readonly string[] CascadedChildren = ["UserProjects", "UserAssignments"];

    public static string ChildTriggerName(string table) => $"{table}_ActiveParents";

    public static string ParentTriggerName(string table) => $"{table}_ActiveDependents";

    protected override void Up(MigrationBuilder migrationBuilder)
    {
        foreach (var child in References.GroupBy(reference => reference.Child))
            migrationBuilder.Sql(ChildTriggerSql(child.Key, child.ToArray()));

        foreach (var parent in References.GroupBy(reference => reference.Parent))
            migrationBuilder.Sql(ParentTriggerSql(parent.Key, parent.ToArray()));
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        foreach (var parent in References.Select(reference => reference.Parent).Distinct())
            migrationBuilder.Sql($"""
                DROP TRIGGER "{ParentTriggerName(parent)}" ON "{parent}";
                DROP FUNCTION {FunctionName("dependents", parent)}();
                """);

        foreach (var child in References.Select(reference => reference.Child).Distinct())
            migrationBuilder.Sql($"""
                DROP TRIGGER "{ChildTriggerName(child)}" ON "{child}";
                DROP FUNCTION {FunctionName("parents", child)}();
                """);
    }

    private static string FunctionName(string kind, string table) => $"cpnucleo_{kind}_{table.ToLowerInvariant()}";

    private static string ChildTriggerSql(string child, (string Child, string Column, string Parent)[] references)
    {
        var checks = string.Join("\n", references.Select(reference => $"""
                IF TG_OP = 'INSERT' OR NOT OLD."Active" OR NEW."{reference.Column}" IS DISTINCT FROM OLD."{reference.Column}" THEN
                    PERFORM 1 FROM "{reference.Parent}" WHERE "Id" = NEW."{reference.Column}" AND "Active" FOR SHARE;
                    IF NOT FOUND THEN
                        RAISE EXCEPTION '{child}.{reference.Column} references a missing or removed {reference.Parent} row.'
                            USING ERRCODE = '23503', TABLE = '{child}', COLUMN = '{reference.Column}';
                    END IF;
                END IF;
            """));

        return $"""
            CREATE FUNCTION {FunctionName("parents", child)}() RETURNS trigger LANGUAGE plpgsql AS $body$
            BEGIN
                IF NOT NEW."Active" THEN RETURN NEW; END IF;
            {checks}
                RETURN NEW;
            END;
            $body$;
            CREATE TRIGGER "{ChildTriggerName(child)}" BEFORE INSERT OR UPDATE ON "{child}"
                FOR EACH ROW EXECUTE FUNCTION {FunctionName("parents", child)}();
            """;
    }

    private static string ParentTriggerSql(string parent, (string Child, string Column, string Parent)[] references)
    {
        var blocking = string.Join("\n", references
            .Where(reference => !CascadedChildren.Contains(reference.Child))
            .Select(reference => $"""
                    IF EXISTS (SELECT 1 FROM "{reference.Child}" WHERE "{reference.Column}" = OLD."Id" AND "Active") THEN
                        RAISE EXCEPTION '{parent} row % still has active {reference.Child}.', OLD."Id"
                            USING ERRCODE = '23001', TABLE = '{reference.Child}';
                    END IF;
            """));

        var cascades = string.Join("\n", references
            .Where(reference => CascadedChildren.Contains(reference.Child))
            .Select(reference => $"""
                    UPDATE "{reference.Child}" SET "Active" = false, "DeletedAt" = COALESCE(NEW."DeletedAt", now())
                    WHERE "{reference.Column}" = OLD."Id" AND "Active";
            """));

        return $"""
            CREATE FUNCTION {FunctionName("dependents", parent)}() RETURNS trigger LANGUAGE plpgsql AS $body$
            BEGIN
                IF OLD."Active" AND NOT NEW."Active" THEN
            {blocking}
            {cascades}
                END IF;
                RETURN NEW;
            END;
            $body$;
            CREATE TRIGGER "{ParentTriggerName(parent)}" BEFORE UPDATE OF "Active" ON "{parent}"
                FOR EACH ROW EXECUTE FUNCTION {FunctionName("dependents", parent)}();
            """;
    }
}
