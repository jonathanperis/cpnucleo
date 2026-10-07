using System.Security.Cryptography;
using Dapper;
using Microsoft.IdentityModel.Tokens;
using Open.IdentityServer.Models;
using Open.IdentityServer.Stores;

namespace IdentityApi.Oidc;

/// <summary>
/// Rotating RS256 key ring, persisted in <c>IdentitySigningKeys</c> with private keys encrypted at rest.
/// <list type="bullet">
/// <item>Each key signs for <see cref="RotationPeriod"/>. Its successor is created
/// <see cref="PublishAhead"/> before that, so API hosts already have it in the published key set
/// (JWKS) when it starts signing.</item>
/// <item>A replaced key stays published for <see cref="RetainAfterRotation"/>, longer than any
/// token it signed can live, then it is deleted.</item>
/// <item><c>Jwt:SigningPrivateKey</c> (PEM) pins one externally managed key instead; nothing is stored.</item>
/// </list>
/// API hosts never hold key material: they read the JWKS from the discovery document.
/// </summary>
public sealed class SigningKeyRing(
    NpgsqlDataSource dataSource,
    IdentityKeyProtector protector,
    IConfiguration configuration,
    TimeProvider timeProvider,
    ILogger<SigningKeyRing> logger) : ISigningCredentialStore, IValidationKeysStore
{
    public static readonly TimeSpan RotationPeriod = TimeSpan.FromDays(90);
    public static readonly TimeSpan PublishAhead = TimeSpan.FromDays(1);
    public static readonly TimeSpan RetainAfterRotation = TimeSpan.FromDays(7);
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);
    private const string Algorithm = SecurityAlgorithms.RsaSha256;
    private const string Purpose = "signing-key";
    private const int KeySize = 2048;

    private readonly SemaphoreSlim gate = new(1, 1);
    private Snapshot? cached;

    public async Task<SigningCredentials> GetSigningCredentialsAsync() =>
        new((await GetSnapshotAsync()).Signing.Key, Algorithm);

    public async Task<IEnumerable<SecurityKeyInfo>> GetValidationKeysAsync() =>
        (await GetSnapshotAsync()).Published.Select(key => new SecurityKeyInfo { Key = key.Key, SigningAlgorithm = Algorithm });

    private async Task<Snapshot> GetSnapshotAsync()
    {
        var now = timeProvider.GetUtcNow();
        if (cached is { } current && current.IsFresh(now)) return current;

        await gate.WaitAsync();
        try
        {
            if (cached is { } loaded && loaded.IsFresh(now)) return loaded;
            cached = configuration["Jwt:SigningPrivateKey"] is { Length: > 0 } pem
                ? Snapshot.Pinned(ImportPem(pem), now)
                : await LoadRingAsync(now);
            return cached;
        }
        finally
        {
            gate.Release();
        }
    }

    private async Task<Snapshot> LoadRingAsync(DateTimeOffset now)
    {
        await using var connection = await dataSource.OpenConnectionAsync();
        await using var transaction = await connection.BeginTransactionAsync();
        // One writer at a time, across instances.
        await connection.ExecuteAsync("SELECT pg_advisory_xact_lock(72419, 1)", transaction: transaction);
        await connection.ExecuteAsync("""DELETE FROM "IdentitySigningKeys" WHERE "RetiresAt" <= @now""", new { now = now.UtcDateTime }, transaction);

        var rows = (await connection.QueryAsync<KeyRow>("""
            SELECT "Id", "ProtectedKey", "ActivatesAt" AS "ActivatesAtUtc", "RetiresAt" AS "RetiresAtUtc" FROM "IdentitySigningKeys" ORDER BY "ActivatesAt"
            """, transaction: transaction)).ToList();

        var active = rows.LastOrDefault(row => row.ActivatesAt <= now);
        if (active is null)
        {
            rows.Add(await CreateKeyAsync(connection, transaction, activatesAt: now));
        }
        else if (active.ActivatesAt + RotationPeriod - PublishAhead <= now && !rows.Any(row => row.ActivatesAt > active.ActivatesAt))
        {
            var activatesAt = active.ActivatesAt + RotationPeriod;
            rows.Add(await CreateKeyAsync(connection, transaction, activatesAt > now ? activatesAt : now));
        }

        await transaction.CommitAsync();

        var keys = rows.Select(row => new RingKey(row.Id, Decrypt(row), row.ActivatesAt)).ToList();
        var signing = keys.Last(key => key.ActivatesAt <= now);
        // Refresh in time to switch to a successor that activates before the cache would expire.
        var nextActivation = keys.Where(key => key.ActivatesAt > now).Select(key => key.ActivatesAt).DefaultIfEmpty(DateTimeOffset.MaxValue).Min();
        return new Snapshot(signing, keys, now, nextActivation);
    }

    private async Task<KeyRow> CreateKeyAsync(NpgsqlConnection connection, NpgsqlTransaction transaction, DateTimeOffset activatesAt)
    {
        using var rsa = RSA.Create(KeySize);
        var row = new KeyRow
        {
            Id = Convert.ToHexStringLower(RandomNumberGenerator.GetBytes(16)),
            ProtectedKey = protector.Protect(rsa.ExportPkcs8PrivateKey(), Purpose),
            ActivatesAtUtc = activatesAt.UtcDateTime,
            RetiresAtUtc = (activatesAt + RotationPeriod + RetainAfterRotation).UtcDateTime
        };
        await connection.ExecuteAsync("""
            INSERT INTO "IdentitySigningKeys" ("Id", "Algorithm", "ProtectedKey", "CreatedAt", "ActivatesAt", "RetiresAt")
            VALUES (@Id, @Algorithm, @ProtectedKey, @CreatedAt, @ActivatesAt, @RetiresAt)
            """, new { row.Id, Algorithm, row.ProtectedKey, CreatedAt = timeProvider.GetUtcNow().UtcDateTime, ActivatesAt = row.ActivatesAtUtc, RetiresAt = row.RetiresAtUtc }, transaction);
        logger.LogInformation("Created signing key {KeyId}, active from {ActivatesAt:O}.", row.Id, row.ActivatesAt);
        return row;
    }

    private RsaSecurityKey Decrypt(KeyRow row)
    {
        using var rsa = RSA.Create();
        rsa.ImportPkcs8PrivateKey(protector.Unprotect(row.ProtectedKey, Purpose), out _);
        return new RsaSecurityKey(rsa.ExportParameters(includePrivateParameters: true)) { KeyId = row.Id };
    }

    private static RsaSecurityKey ImportPem(string pem)
    {
        using var rsa = RSA.Create();
        rsa.ImportFromPem(pem.Replace("\\n", "\n", StringComparison.Ordinal).Trim());
        var parameters = rsa.ExportParameters(includePrivateParameters: true);
        return new RsaSecurityKey(parameters) { KeyId = KeyIdFor(parameters) };
    }

    /// <summary>A stable key id derived from the public key, so every instance publishes the same <c>kid</c>.</summary>
    public static string KeyIdFor(RSAParameters parameters) =>
        Convert.ToHexStringLower(SHA256.HashData([.. parameters.Modulus!, .. parameters.Exponent!]))[..32];

    private sealed class KeyRow
    {
        public string Id { get; init; } = "";
        public byte[] ProtectedKey { get; init; } = [];
        // timestamptz arrives as a UTC DateTime.
        public DateTime ActivatesAtUtc { get; init; }
        public DateTime RetiresAtUtc { get; init; }
        public DateTimeOffset ActivatesAt => new(DateTime.SpecifyKind(ActivatesAtUtc, DateTimeKind.Utc));
    }

    private sealed record RingKey(string Id, RsaSecurityKey Key, DateTimeOffset ActivatesAt);

    private sealed record Snapshot(RingKey Signing, IReadOnlyList<RingKey> Published, DateTimeOffset LoadedAt, DateTimeOffset NextActivation)
    {
        public static Snapshot Pinned(RsaSecurityKey key, DateTimeOffset now)
        {
            var only = new RingKey(key.KeyId, key, now);
            return new Snapshot(only, [only], now, DateTimeOffset.MaxValue);
        }

        public bool IsFresh(DateTimeOffset now) => now - LoadedAt < CacheDuration && now < NextActivation;
    }
}
