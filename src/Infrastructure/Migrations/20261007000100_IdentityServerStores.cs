using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations;

/// <summary>
/// Storage for the OpenID Connect server in IdentityApi (Open.IdentityServer). The tables are
/// written with Dapper by IdentityApi and are not part of the EF Core model:
/// <list type="bullet">
/// <item><c>IdentityPersistedGrants</c>: authorization codes and refresh tokens (one-time use, so
/// a consumed refresh token stays until it expires to detect reuse).</item>
/// <item><c>IdentitySessions</c>: one row per sign-in (the OIDC <c>sid</c>). API hosts reject tokens
/// whose session ended, so signing out revokes access tokens too.</item>
/// <item><c>IdentitySigningKeys</c>: the rotating RS256 key ring published as JWKS. Private keys
/// are encrypted at rest.</item>
/// <item><c>IdentityDataProtectionKeys</c>: ASP.NET Core Data Protection keys (identity cookies),
/// encrypted at rest, so a restart does not sign everyone out.</item>
/// </list>
/// Sessions reference users by id without a foreign key: users are only soft-deleted, and the seed
/// tool truncates <c>Users</c>.
/// </summary>
[DbContext(typeof(ApplicationDbContext))]
[Migration("20261007000100_IdentityServerStores")]
public sealed class IdentityServerStores : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            CREATE TABLE "IdentityPersistedGrants" (
                "Key" varchar(200) PRIMARY KEY,
                "Type" varchar(50) NOT NULL,
                "SubjectId" varchar(200),
                "SessionId" varchar(100),
                "ClientId" varchar(200) NOT NULL,
                "Description" varchar(200),
                "CreationTime" timestamptz NOT NULL,
                "Expiration" timestamptz,
                "ConsumedTime" timestamptz,
                "Data" text NOT NULL
            );
            CREATE INDEX "IX_IdentityPersistedGrants_Subject_Session_Type" ON "IdentityPersistedGrants" ("SubjectId", "SessionId", "Type");
            CREATE INDEX "IX_IdentityPersistedGrants_Subject_Client_Type" ON "IdentityPersistedGrants" ("SubjectId", "ClientId", "Type");
            CREATE INDEX "IX_IdentityPersistedGrants_Expiration" ON "IdentityPersistedGrants" ("Expiration");

            CREATE TABLE "IdentitySessions" (
                "Id" varchar(100) PRIMARY KEY,
                "UserId" uuid NOT NULL,
                "StartedAt" timestamptz NOT NULL,
                "EndedAt" timestamptz
            );
            CREATE INDEX "IX_IdentitySessions_UserId" ON "IdentitySessions" ("UserId");

            CREATE TABLE "IdentitySigningKeys" (
                "Id" varchar(64) PRIMARY KEY,
                "Algorithm" varchar(16) NOT NULL,
                "ProtectedKey" bytea NOT NULL,
                "CreatedAt" timestamptz NOT NULL,
                "ActivatesAt" timestamptz NOT NULL,
                "RetiresAt" timestamptz NOT NULL
            );

            CREATE TABLE "IdentityDataProtectionKeys" (
                "Id" integer GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
                "FriendlyName" text,
                "Xml" text NOT NULL
            );
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            DROP TABLE "IdentityDataProtectionKeys";
            DROP TABLE "IdentitySigningKeys";
            DROP TABLE "IdentitySessions";
            DROP TABLE "IdentityPersistedGrants";
            """);
    }
}
