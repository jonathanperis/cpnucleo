namespace Security.Unit.Tests;

[TestFixture]
public class UserPasswordHashTests
{
    [Test]
    public void Create_ShouldStoreProvidedPasswordHashAndEmptySalt()
    {
        var user = User.Create("Jane", "jane", new PasswordHash("hash-value", string.Empty));

        user.Password.ShouldBe("hash-value");
        user.Salt.ShouldBeEmpty();
    }

    [Test]
    public void Update_ShouldStoreProvidedPasswordHashAndEmptySalt()
    {
        var user = User.Create("Jane", "jane", new PasswordHash("old-hash", string.Empty));

        User.Update(user, "Jane Updated", new PasswordHash("new-hash", string.Empty));

        user.Name.ShouldBe("Jane Updated");
        user.Password.ShouldBe("new-hash");
        user.Salt.ShouldBeEmpty();
        user.UpdatedAt.ShouldNotBeNull();
    }
}
