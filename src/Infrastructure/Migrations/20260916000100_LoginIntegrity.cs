using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations;

[DbContext(typeof(ApplicationDbContext))]
[Migration("20260916000100_LoginIntegrity")]
public sealed class LoginIntegrity : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Existing ambiguous identities remain untouched. New/changed active logins
        // are serialized and compared by normalized key for both EF and Dapper.
        migrationBuilder.Sql("""
            CREATE INDEX "IX_Users_ActiveNormalizedLogin" ON "Users" (lower(btrim("Login"))) WHERE "Active";
            CREATE FUNCTION cpnucleo_check_login() RETURNS trigger LANGUAGE plpgsql AS $body$
            BEGIN
                IF NOT NEW."Active" THEN RETURN NEW; END IF;
                IF TG_OP = 'UPDATE' AND NEW."Login" IS NOT DISTINCT FROM OLD."Login"
                    AND NEW."Active" IS NOT DISTINCT FROM OLD."Active" THEN RETURN NEW; END IF;
                IF NEW."Login" IS NULL OR btrim(NEW."Login") = '' THEN
                    RAISE EXCEPTION 'Login is required' USING ERRCODE = '23514';
                END IF;
                PERFORM pg_advisory_xact_lock(72419, 0);
                IF EXISTS (SELECT 1 FROM "Users" WHERE "Active" AND "Id" <> NEW."Id"
                    AND lower(btrim("Login")) = lower(btrim(NEW."Login"))) THEN
                    RAISE EXCEPTION 'Login is already in use' USING ERRCODE = '23505', CONSTRAINT = 'Users_ActiveLogin';
                END IF;
                RETURN NEW;
            END;
            $body$;
            CREATE TRIGGER "Users_LoginIntegrity" BEFORE INSERT OR UPDATE OF "Login", "Active" ON "Users"
                FOR EACH ROW EXECUTE FUNCTION cpnucleo_check_login();
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            DROP TRIGGER "Users_LoginIntegrity" ON "Users";
            DROP FUNCTION cpnucleo_check_login();
            DROP INDEX "IX_Users_ActiveNormalizedLogin";
            """);
    }
}
