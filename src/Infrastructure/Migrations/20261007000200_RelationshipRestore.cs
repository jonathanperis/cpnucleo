using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations;

/// <summary>
/// Restoring (undoing the soft delete of) a parent brings back the membership links that
/// <see cref="RelationshipIntegrity"/> removed together with it: link rows of the parent whose
/// <c>DeletedAt</c> equals the parent's, and whose other parents are active. Links removed on their
/// own earlier stay removed. The trigger runs after the parent row changed, so the links' own
/// <c>ActiveParents</c> checks see the restored parent. Additive: existing triggers are unchanged.
/// </summary>
[DbContext(typeof(ApplicationDbContext))]
[Migration("20261007000200_RelationshipRestore")]
public sealed class RelationshipRestore : Migration
{
    public static string TriggerName(string table) => $"{table}_RestoreLinks";

    /// <summary>(parent table, link table, column) for every cascaded link.</summary>
    private static IEnumerable<IGrouping<string, (string Child, string Column, string Parent)>> CascadingParents() =>
        RelationshipIntegrity.References
            .Where(reference => RelationshipIntegrity.CascadedChildren.Contains(reference.Child))
            .GroupBy(reference => reference.Parent);

    protected override void Up(MigrationBuilder migrationBuilder)
    {
        foreach (var parent in CascadingParents())
            migrationBuilder.Sql(RestoreTriggerSql(parent.Key, parent.ToArray()));
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        foreach (var parent in CascadingParents())
            migrationBuilder.Sql($"""
                DROP TRIGGER "{TriggerName(parent.Key)}" ON "{parent.Key}";
                DROP FUNCTION {FunctionName(parent.Key)}();
                """);
    }

    private static string FunctionName(string table) => $"cpnucleo_restore_{table.ToLowerInvariant()}";

    private static string RestoreTriggerSql(string parent, (string Child, string Column, string Parent)[] links)
    {
        var restores = string.Join("\n", links.Select(link =>
        {
            var otherParents = RelationshipIntegrity.References
                .Where(reference => reference.Child == link.Child && reference.Column != link.Column)
                .Select(reference => $"""
                        AND EXISTS (SELECT 1 FROM "{reference.Parent}" p WHERE p."Id" = c."{reference.Column}" AND p."Active")
                """);
            return $"""
                    UPDATE "{link.Child}" c SET "Active" = true, "DeletedAt" = NULL
                    WHERE c."{link.Column}" = NEW."Id" AND NOT c."Active" AND c."DeletedAt" = OLD."DeletedAt"
                {string.Join("\n", otherParents)};
                """;
        }));

        return $"""
            CREATE FUNCTION {FunctionName(parent)}() RETURNS trigger LANGUAGE plpgsql AS $body$
            BEGIN
            {restores}
                RETURN NULL;
            END;
            $body$;
            CREATE TRIGGER "{TriggerName(parent)}" AFTER UPDATE OF "Active" ON "{parent}"
                FOR EACH ROW WHEN (NOT OLD."Active" AND NEW."Active")
                EXECUTE FUNCTION {FunctionName(parent)}();
            """;
    }
}
