namespace Security.Unit.Tests;

[TestFixture]
public class Argon2PasswordHasherTests
{
    [Test]
    public void Hash_ShouldReturnArgon2idPhcHashAndEmptySalt()
    {
        var hasher = new Argon2PasswordHasher();

        var passwordHash = hasher.Hash("Password@123");

        passwordHash.Hash.ShouldStartWith("$argon2id$");
        passwordHash.Hash.ShouldContain("$m=65536,t=3,p=2$");
        passwordHash.Salt.ShouldBeEmpty();
    }

    [Test]
    public void Verify_ShouldAcceptCorrectArgon2Password()
    {
        var hasher = new Argon2PasswordHasher();
        var passwordHash = hasher.Hash("Password@123");

        var verified = hasher.Verify("Password@123", passwordHash.Hash);

        verified.ShouldBeTrue();
    }

    [Test]
    public void Verify_ShouldRejectWrongArgon2Password()
    {
        var hasher = new Argon2PasswordHasher();
        var passwordHash = hasher.Hash("Password@123");

        var verified = hasher.Verify("WrongPassword@123", passwordHash.Hash);

        verified.ShouldBeFalse();
    }

    [Test]
    public void Verify_ShouldRejectNonArgon2Hash()
    {
        var hasher = new Argon2PasswordHasher();

        var verified = hasher.Verify("Password@123", Convert.ToBase64String(Guid.NewGuid().ToByteArray()));

        verified.ShouldBeFalse();
    }

    [Test]
    public void Verify_ShouldReturnFalseForMalformedStoredValues()
    {
        var hasher = new Argon2PasswordHasher();

        var verified = hasher.Verify("Password@123", "$argon2id$v=19$m=bad,t=3,p=2$not-base64$not-base64");

        verified.ShouldBeFalse();
    }

    [Test]
    public void Verify_ShouldRejectExcessiveStoredParameters()
    {
        var hasher = new Argon2PasswordHasher();
        var salt = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        var hash = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

        var verified = hasher.Verify("Password@123", $"$argon2id$v=19$m=999999,t=3,p=2${salt}${hash}");

        verified.ShouldBeFalse();
    }
}
