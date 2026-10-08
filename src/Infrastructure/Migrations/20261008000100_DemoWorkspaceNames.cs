using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations;

/// <summary>
/// Gives databases seeded with the Bogus demo dataset before <see cref="DemoWorkspaceNames"/> existed
/// their workspace names, once, through the normal one-shot migrator. The script only rewrites rows
/// that still carry generated text, so it is a no-op on empty, lab and already renamed databases and
/// never touches people's rows, Ids, relations or lifecycle columns. Rewriting the full dataset
/// through the trigram search indexes row by row is slow, so they are rebuilt in bulk around it.
/// The drops keep their locks until the migration commits, so searches wait instead of running
/// without the indexes.
/// </summary>
[DbContext(typeof(ApplicationDbContext))]
[Migration("20261008000100_DemoWorkspaceNames")]
public sealed class DemoWorkspaceNamesData : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        foreach (var (table, column) in ListAndSearchIndexes.SearchColumns)
            migrationBuilder.Sql(ListAndSearchIndexes.DropTrigramIndexSql(table, column));

        migrationBuilder.Sql(DemoWorkspaceNames.Script);

        foreach (var (table, column) in ListAndSearchIndexes.SearchColumns)
            migrationBuilder.Sql(ListAndSearchIndexes.CreateTrigramIndexSql(table, column));
    }

    // The replaced Bogus text has no value and is not kept, so there is nothing to restore.
    protected override void Down(MigrationBuilder migrationBuilder)
    {
    }
}
