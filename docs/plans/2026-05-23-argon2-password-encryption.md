# Argon2 Password Encryption Implementation Plan

> **For Hermes:** Use subagent-driven-development skill to implement this plan task-by-task.

**Goal:** Replace new password hashing from PBKDF2 to Argon2id while preserving login access for existing PBKDF2-hashed users via verify-and-rehash migration.

**Architecture:** Keep Domain free of external cryptography packages by moving password hashing behind a small Domain-owned `IPasswordHasher` abstraction implemented in Infrastructure. New/updated users receive Argon2id PHC-formatted hashes in `Users.Password`; existing PBKDF2 rows continue to verify using their `Users.Salt` value and are upgraded to Argon2id on successful login.

**Tech Stack:** .NET 10, C#, EF Core, Dapper, FastEndpoints, `Konscious.Security.Cryptography.Argon2` 1.3.1, xUnit/NUnit tests.

---

## Inspection Snapshot

- **Repo/path inspected:** `/opt/data/github/jonathanperis/cpnucleo`
- **Remote:** `origin https://github.com/jonathanperis/cpnucleo.git`
- **Base branch/ref:** `main` at `6066cb3af79627c7c89c2a43d957eae02b647f0c` (`chore: remove Hostinger open redirect helper`), equal to `origin/main` after `git fetch origin main --prune`
- **Planning branch:** `docs/plan-argon2-password-encryption`
- **Working tree before plan:** clean
- **Current implementation evidence:**
  - `src/Domain/Common/Security/CryptographyManager.cs` hashes PBKDF2 with 48-byte random salt, 600,000 iterations, SHA-256, 48-byte output.
  - `src/Domain/Entities/User.cs` calls `CryptographyManager.CryptPbkdf2()` inside `Create()` and `Update()`.
  - `src/IdentityApi/Endpoints/Login/Endpoint.cs` currently queries `u.Login == req.Login && u.Password == req.Password`, which cannot verify hashed passwords and must change to fetch by login then verify.
  - `Users.Password` and `Users.Salt` are nullable PostgreSQL `text`, so Argon2 PHC strings fit without a schema migration.
- **External package fact checked:** NuGet search shows `Konscious.Security.Cryptography.Argon2` latest stable `1.3.1`.
- **Implementation status:** not implemented; this is a plan only.

---

## Design Decisions

1. **Use Argon2id, not Argon2i/d:** Argon2id is the recommended general-purpose password hashing variant.
2. **Store Argon2 as PHC in `Users.Password`:** Example shape: `$argon2id$v=19$m=65536,t=3,p=2$<base64-salt>$<base64-hash>`. This records parameters with the hash, enabling future tuning.
3. **Keep `Users.Salt` for PBKDF2 compatibility:** New Argon2 rows set `Salt = string.Empty`. Existing PBKDF2 rows keep the old base64 salt until the user logs in successfully and is rehashed.
4. **Preserve Clean Architecture:** Domain defines password-hashing contracts but does not reference `Konscious.Security.Cryptography.Argon2`; Infrastructure contains the implementation and NuGet dependency.
5. **Rehash on successful legacy login:** When PBKDF2 verifies, IdentityApi writes a new Argon2 hash immediately and clears `Salt`.
6. **Use fixed-time comparison:** Verifiers must use `CryptographicOperations.FixedTimeEquals`, not `SequenceEqual`.

---

## Target File Changes

- Create `src/Domain/Common/Security/IPasswordHasher.cs`
- Create `src/Domain/Common/Security/PasswordHash.cs`
- Create `src/Domain/Common/Security/PasswordVerificationResult.cs`
- Modify `src/Domain/Common/Security/CryptographyManager.cs` (remove or stop using PBKDF2 production entry points; keep no external deps)
- Modify `src/Domain/Entities/User.cs`
- Modify `src/Infrastructure/Infrastructure.csproj`
- Create `src/Infrastructure/Common/Security/Argon2PasswordHasher.cs`
- Modify `src/Infrastructure/DependencyInjection.cs`
- Modify `src/Infrastructure/Usings.cs`
- Modify `src/WebApi/Endpoints/User/CreateUser/Endpoint.cs`
- Modify `src/WebApi/Endpoints/User/UpdateUser/Endpoint.cs`
- Modify `src/GrpcServer/Handlers/User/CreateUserHandler.cs`
- Modify `src/GrpcServer/Handlers/User/UpdateUserHandler.cs`
- Modify `src/IdentityApi/Endpoints/Login/Endpoint.cs`
- Add tests under either existing suites or new focused projects as described below.

---

## Task 1: Add password hashing contracts to Domain

**Objective:** Define stable contracts without pulling Argon2 packages into Domain.

**Files:**
- Create: `src/Domain/Common/Security/PasswordHash.cs`
- Create: `src/Domain/Common/Security/PasswordVerificationResult.cs`
- Create: `src/Domain/Common/Security/IPasswordHasher.cs`

**Step 1: Create the value objects and interface**

```csharp
namespace Domain.Common.Security;

public sealed record PasswordHash(string Hash, string Salt);
```

```csharp
namespace Domain.Common.Security;

public sealed record PasswordVerificationResult(bool Verified, bool NeedsRehash);
```

```csharp
namespace Domain.Common.Security;

public interface IPasswordHasher
{
    PasswordHash Hash(string? password);

    PasswordVerificationResult Verify(string? password, string? hash, string? salt);
}
```

**Step 2: Build**

Run:

```bash
dotnet build cpnucleo.slnx
```

Expected: build succeeds; existing expected FakeData warnings are acceptable.

**Step 3: Commit**

```bash
git add src/Domain/Common/Security/PasswordHash.cs src/Domain/Common/Security/PasswordVerificationResult.cs src/Domain/Common/Security/IPasswordHasher.cs
git commit -m "feat: add password hashing contract"
```

---

## Task 2: Make User entity accept already-hashed passwords

**Objective:** Remove hashing work from Domain entity factories so Domain stays package-free.

**Files:**
- Modify: `src/Domain/Entities/User.cs`

**Step 1: Write or update tests first**

If no Domain unit-test project exists, create `test/Domain.Unit.Tests/Domain.Unit.Tests.csproj` referencing `src/Domain/Domain.csproj`, xUnit, and FluentAssertions. Add tests covering:

```csharp
[Fact]
public void Create_ShouldStoreProvidedPasswordHashAndSalt()
{
    var user = User.Create("Jane", "jane", new PasswordHash("hash-value", "salt-value"));

    user.Password.Should().Be("hash-value");
    user.Salt.Should().Be("salt-value");
}

[Fact]
public void Update_ShouldStoreProvidedPasswordHashAndSalt()
{
    var user = User.Create("Jane", "jane", new PasswordHash("old-hash", "old-salt"));

    User.Update(user, "Jane Updated", new PasswordHash("new-hash", "new-salt"));

    user.Name.Should().Be("Jane Updated");
    user.Password.Should().Be("new-hash");
    user.Salt.Should().Be("new-salt");
    user.UpdatedAt.Should().NotBeNull();
}
```

Run and verify RED:

```bash
dotnet test test/Domain.Unit.Tests/Domain.Unit.Tests.csproj --filter "Create_ShouldStoreProvidedPasswordHashAndSalt|Update_ShouldStoreProvidedPasswordHashAndSalt"
```

Expected: fails because `User.Create` / `User.Update` still accept plaintext password strings.

**Step 2: Change `User` factory signatures**

Change:

```csharp
public static User Create(string? name, string? login, string? password, Guid id = default)
```

To:

```csharp
public static User Create(string? name, string? login, PasswordHash passwordHash, Guid id = default)
```

And set:

```csharp
Password = passwordHash.Hash,
Salt = passwordHash.Salt,
```

Change:

```csharp
public static void Update(User obj, string? name, string? password)
```

To:

```csharp
public static void Update(User obj, string? name, PasswordHash passwordHash)
```

And set:

```csharp
obj.Password = passwordHash.Hash;
obj.Salt = passwordHash.Salt;
```

**Step 3: Verify GREEN**

```bash
dotnet test test/Domain.Unit.Tests/Domain.Unit.Tests.csproj
```

Expected: pass.

**Step 4: Commit**

```bash
git add src/Domain/Entities/User.cs test/Domain.Unit.Tests
git commit -m "refactor: keep password hashing outside user entity"
```

---

## Task 3: Implement Argon2id hasher with PBKDF2 fallback in Infrastructure

**Objective:** Add a production `IPasswordHasher` implementation that creates Argon2id hashes and verifies both Argon2id and legacy PBKDF2 rows.

**Files:**
- Modify: `src/Infrastructure/Infrastructure.csproj`
- Modify: `src/Infrastructure/Usings.cs`
- Create: `src/Infrastructure/Common/Security/Argon2PasswordHasher.cs`
- Test: create `test/Infrastructure.Unit.Tests/Infrastructure.Unit.Tests.csproj` or place equivalent focused tests in an existing acceptable test project.

**Step 1: Add package**

```bash
dotnet add src/Infrastructure/Infrastructure.csproj package Konscious.Security.Cryptography.Argon2 --version 1.3.1
```

**Step 2: Write failing hasher tests**

Test behaviors:

1. `Hash_ShouldReturnArgon2idPhcHashAndEmptySalt`
2. `Verify_ShouldAcceptCorrectArgon2Password`
3. `Verify_ShouldRejectWrongArgon2Password`
4. `Verify_ShouldAcceptLegacyPbkdf2AndMarkNeedsRehash`
5. `Verify_ShouldReturnFalseForMalformedStoredValues`

PBKDF2 fixture helper for test 4:

```csharp
private static PasswordHash CreateLegacyPbkdf2(string password)
{
    byte[] saltBytes = RandomNumberGenerator.GetBytes(48);
    byte[] hashBytes = Rfc2898DeriveBytes.Pbkdf2(password, saltBytes, 600_000, HashAlgorithmName.SHA256, 48);
    return new PasswordHash(Convert.ToBase64String(hashBytes), Convert.ToBase64String(saltBytes));
}
```

Run RED:

```bash
dotnet test test/Infrastructure.Unit.Tests/Infrastructure.Unit.Tests.csproj --filter "Argon2PasswordHasher"
```

Expected: fails because `Argon2PasswordHasher` does not exist.

**Step 3: Implement `Argon2PasswordHasher`**

Core constants:

```csharp
private const int SaltSize = 16;
private const int HashSize = 32;
private const int MemorySize = 65_536; // KiB = 64 MiB
private const int Iterations = 3;
private const int DegreeOfParallelism = 2;
private const string Argon2idPrefix = "$argon2id$";
```

Required implementation points:

- `Hash()` returns `new PasswordHash(phcString, string.Empty)`.
- `Verify()` detects `hash.StartsWith("$argon2id$", StringComparison.Ordinal)` and verifies Argon2id.
- Otherwise, `Verify()` attempts legacy PBKDF2 using provided `salt`; on success returns `new PasswordVerificationResult(true, true)`.
- Argon2id success returns `new PasswordVerificationResult(true, false)`.
- All failed/malformed paths return `new PasswordVerificationResult(false, false)`.
- Use `CryptographicOperations.FixedTimeEquals` for both Argon2 and PBKDF2 byte comparison.

Pseudo-shape:

```csharp
public sealed class Argon2PasswordHasher : IPasswordHasher
{
    public PasswordHash Hash(string? password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return new PasswordHash(string.Empty, string.Empty);
        }

        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = HashArgon2id(password, salt, MemorySize, Iterations, DegreeOfParallelism, HashSize);
        var phc = $"$argon2id$v=19$m={MemorySize},t={Iterations},p={DegreeOfParallelism}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
        return new PasswordHash(phc, string.Empty);
    }

    public PasswordVerificationResult Verify(string? password, string? hash, string? salt)
    {
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hash))
        {
            return new PasswordVerificationResult(false, false);
        }

        if (hash.StartsWith(Argon2idPrefix, StringComparison.Ordinal))
        {
            return VerifyArgon2id(password, hash);
        }

        return VerifyLegacyPbkdf2(password, hash, salt);
    }
}
```

**Step 4: Verify GREEN**

```bash
dotnet test test/Infrastructure.Unit.Tests/Infrastructure.Unit.Tests.csproj --filter "Argon2PasswordHasher"
dotnet test test/Architecture.Tests/
```

Expected: hasher tests pass; architecture tests pass.

**Step 5: Commit**

```bash
git add src/Infrastructure/Infrastructure.csproj src/Infrastructure/Usings.cs src/Infrastructure/Common/Security/Argon2PasswordHasher.cs test/Infrastructure.Unit.Tests
git commit -m "feat: add argon2 password hasher"
```

---

## Task 4: Register password hasher in DI

**Objective:** Make `IPasswordHasher` injectable into WebApi, IdentityApi, and GrpcServer through `AddInfrastructure()`.

**Files:**
- Modify: `src/Infrastructure/DependencyInjection.cs`

**Step 1: Add registration**

In `AddInfrastructure()` near other service registrations:

```csharp
services.AddSingleton<IPasswordHasher, Argon2PasswordHasher>();
```

Add missing using/global using as needed:

```csharp
global using Domain.Common.Security;
global using Infrastructure.Common.Security;
```

**Step 2: Verify**

```bash
dotnet build cpnucleo.slnx
```

Expected: build succeeds.

**Step 3: Commit**

```bash
git add src/Infrastructure/DependencyInjection.cs src/Infrastructure/Usings.cs
git commit -m "chore: register password hasher"
```

---

## Task 5: Hash passwords before creating/updating users in WebApi

**Objective:** Update REST user endpoints to pass already-hashed values to Domain.

**Files:**
- Modify: `src/WebApi/Endpoints/User/CreateUser/Endpoint.cs`
- Modify: `src/WebApi/Endpoints/User/UpdateUser/Endpoint.cs`

**Step 1: Write failing endpoint/unit tests**

Add/update tests to assert:

- Create user stores a value starting with `$argon2id$`, not the plaintext request password.
- Create user stores empty `Salt` for Argon2 rows.
- Update user changes `Password` to an Argon2 hash and clears `Salt`.

Run the specific tests and verify RED.

**Step 2: Inject `IPasswordHasher`**

Change constructors:

```csharp
public class Endpoint(IApplicationDbContext dbContext, IPasswordHasher passwordHasher) : Endpoint<Request, Response>
```

Before `User.Create()`:

```csharp
var passwordHash = passwordHasher.Hash(request.Password);
var newItem = Domain.Entities.User.Create(request.Name, request.Login, passwordHash, request.Id);
```

Before `User.Update()`:

```csharp
var passwordHash = passwordHasher.Hash(request.Password);
Domain.Entities.User.Update(item, request.Name, passwordHash);
```

**Step 3: Verify GREEN**

```bash
dotnet test test/WebApi.Unit.Tests/ --filter "User"
dotnet test test/Architecture.Tests/
```

Expected: user endpoint tests pass; architecture tests pass.

**Step 4: Commit**

```bash
git add src/WebApi/Endpoints/User/CreateUser/Endpoint.cs src/WebApi/Endpoints/User/UpdateUser/Endpoint.cs test/WebApi.Unit.Tests
git commit -m "feat: hash REST user passwords with argon2"
```

---

## Task 6: Hash passwords before creating/updating users in GrpcServer

**Objective:** Update gRPC user handlers to pass already-hashed values to Domain.

**Files:**
- Modify: `src/GrpcServer/Handlers/User/CreateUserHandler.cs`
- Modify: `src/GrpcServer/Handlers/User/UpdateUserHandler.cs`

**Step 1: Write failing handler tests if a gRPC handler test harness exists**

Assert the same behaviors as REST: plaintext is never stored, Argon2 PHC hash is stored, `Salt` is empty for new Argon2 rows.

If no handler test harness exists, at minimum rely on compiler errors from Task 2 plus full solution build, but prefer adding focused tests.

**Step 2: Inject `IPasswordHasher`**

Change constructors:

```csharp
public sealed class CreateUserHandler(
    IUnitOfWork unitOfWork,
    ILogger<CreateUserHandler> logger,
    IPasswordHasher passwordHasher) : ICommandHandler<CreateUserCommand, CreateUserResult>
```

```csharp
var passwordHash = passwordHasher.Hash(command.Password);
var newItem = Domain.Entities.User.Create(command.Name, command.Login, passwordHash, command.Id);
```

Do the same for `UpdateUserHandler` before `User.Update()`.

**Step 3: Verify**

```bash
dotnet build cpnucleo.slnx
dotnet test test/Architecture.Tests/
```

Expected: build succeeds; architecture tests pass.

**Step 4: Commit**

```bash
git add src/GrpcServer/Handlers/User/CreateUserHandler.cs src/GrpcServer/Handlers/User/UpdateUserHandler.cs
git commit -m "feat: hash grpc user passwords with argon2"
```

---

## Task 7: Fix IdentityApi login verification and legacy rehash

**Objective:** Replace plaintext equality login with password verifier and automatic PBKDF2-to-Argon2 upgrade.

**Files:**
- Modify: `src/IdentityApi/Endpoints/Login/Endpoint.cs`

**Step 1: Write failing login tests**

Add focused tests covering:

1. Argon2-hashed user can log in with the correct password.
2. Argon2-hashed user cannot log in with the wrong password.
3. Legacy PBKDF2 user can log in with the correct password.
4. Legacy PBKDF2 user is rehashed to `$argon2id$...` and `Salt == string.Empty` after successful login.

Run RED. Expected: current code fails because it compares stored hash to plaintext.

**Step 2: Inject hasher and verify after lookup**

Change constructor:

```csharp
public class Endpoint(IApplicationDbContext dbContext, IPasswordHasher passwordHasher) : Endpoint<Request, Response>
```

Replace the current query:

```csharp
var item = await dbContext.Users!
    .FirstOrDefaultAsync(u => u.Login == req.Login && u.Password == req.Password, cancellationToken);
```

With:

```csharp
var item = await dbContext.Users!
    .FirstOrDefaultAsync(u => u.Login == req.Login, cancellationToken);

if (item is null)
{
    Logger.LogWarning("User not found with Login: {UserLogin}", req.Login);
    await Send.NotFoundAsync(cancellation: cancellationToken);
    return;
}

var verification = passwordHasher.Verify(req.Password, item.Password, item.Salt);
if (!verification.Verified)
{
    Logger.LogWarning("Invalid password for Login: {UserLogin}", req.Login);
    await Send.NotFoundAsync(cancellation: cancellationToken);
    return;
}

if (verification.NeedsRehash)
{
    var upgradedHash = passwordHasher.Hash(req.Password);
    Domain.Entities.User.Update(item, item.Name, upgradedHash);
    await dbContext.SaveChangesAsync(cancellationToken);
}
```

Then continue JWT creation.

**Step 3: Verify GREEN**

```bash
dotnet test test/IdentityApi.Unit.Tests/ --filter "Login"
```

If no IdentityApi test project exists yet, create one or add integration tests that exercise `/api/login`.

**Step 4: Commit**

```bash
git add src/IdentityApi/Endpoints/Login/Endpoint.cs test/IdentityApi.Unit.Tests
git commit -m "feat: verify argon2 passwords on login"
```

---

## Task 8: Update fake data generation to produce login-compatible hashes

**Objective:** Avoid seed/fake users with random unhashed password strings that cannot pass login verification.

**Files:**
- Modify: `src/Infrastructure/Common/Helpers/FakeData.cs`

**Step 1: Decide fake password policy**

Use one deterministic documented fake password for generated users, for example:

```text
FakeUser@123
```

Hash it with `Argon2PasswordHasher` when generating fake rows. Keep the plaintext only in developer docs/logs if needed.

**Step 2: Add test or smoke script**

Add a focused test/helper assertion that generated fake user rows use `$argon2id$` in `Password` and empty `Salt`.

**Step 3: Implement minimal change**

Replace the user faker password/salt rules with hashed values. Avoid generating invalid random salt strings.

**Step 4: Verify**

```bash
dotnet test test/Infrastructure.Unit.Tests/ --filter "FakeData"
dotnet build cpnucleo.slnx
```

Expected: pass/build succeeds with only known FakeData warnings if they still exist.

**Step 5: Commit**

```bash
git add src/Infrastructure/Common/Helpers/FakeData.cs test/Infrastructure.Unit.Tests
git commit -m "chore: generate argon2 fake user passwords"
```

---

## Task 9: Documentation and API notes

**Objective:** Document the password-hashing migration behavior for maintainers.

**Files:**
- Modify: `README.md` or `docs/wiki/architecture.md`
- Modify: `docs/wiki/database.md` if it documents `Users.Password` / `Users.Salt`

**Step 1: Add concise docs**

Document:

- New users use Argon2id PHC hashes in `Users.Password`.
- `Users.Salt` is retained for legacy PBKDF2 rows and empty for new Argon2 rows.
- Legacy PBKDF2 hashes are upgraded on successful login.
- Do not compare plaintext passwords in queries.

**Step 2: Verify docs build if available**

```bash
cd docs && npm install && npm run build
```

If the docs site dependencies are not installed or the repo convention uses another command, use the documented project command instead.

**Step 3: Commit**

```bash
git add README.md docs/wiki/architecture.md docs/wiki/database.md
git commit -m "docs: document argon2 password hashing"
```

---

## Task 10: Final validation and PR

**Objective:** Prove the migration is safe and ready for review.

**Step 1: Run required gates**

```bash
dotnet build cpnucleo.slnx
dotnet test test/Architecture.Tests/
dotnet test cpnucleo.slnx
```

Expected:

- Build succeeds.
- Architecture tests pass.
- Full test suite passes or any pre-existing skipped/commented suites are explicitly documented.
- No plaintext password comparisons remain in source.

**Step 2: Search for unsafe leftovers**

```bash
rg "Pbkdf2|CryptPbkdf2|VerifyPbkdf2|u\.Password == req\.Password|SequenceEqual\(.*password|Password ==" src test
```

Expected:

- No production PBKDF2 creation path remains.
- Only intentional legacy PBKDF2 verification fallback remains.
- No plaintext password equality query remains.

**Step 3: Open PR**

```bash
git status --short
git push -u origin feat/argon2-password-encryption
gh pr create --base main --head feat/argon2-password-encryption \
  --title "feat: migrate password hashing to Argon2" \
  --body "Migrates new password hashes to Argon2id, keeps PBKDF2 legacy verification, and rehashes legacy users after successful login."
```

**Step 4: Monitor checks**

```bash
gh pr checks --watch
```

Expected: all required checks pass before merge. Use rebase merge only when approved:

```bash
gh pr merge --rebase --delete-branch
```

---

## Risk Checklist

- [ ] Argon2 dependency is only in Infrastructure, not Domain.
- [ ] `Users.Password` never stores plaintext.
- [ ] Login never filters by plaintext password.
- [ ] Existing PBKDF2 users can still log in once.
- [ ] Successful PBKDF2 login upgrades the row to Argon2id.
- [ ] Wrong password fails for Argon2 and PBKDF2 hashes.
- [ ] Malformed stored hashes fail closed, not with a 500.
- [ ] Architecture tests pass.
- [ ] Fake/seed data remains compatible with login behavior.
