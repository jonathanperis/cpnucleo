using System.Xml.Linq;
using IdentityApi.Oidc;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Security.Unit.Tests;

/// <summary>Identity cookies' Data Protection keys are stored encrypted, and survive a secret change.</summary>
public class DataProtectionKeyEncryptionTests
{
    [Test]
    public void StoredKeys_AreEncryptedAndUsableWithTheSameSecret()
    {
        var repository = new InMemoryRepository();
        var first = Protector(repository, TestSupport.Configuration());
        var token = first.Protect("cookie");

        repository.Elements.ShouldNotBeEmpty();
        repository.Elements.ShouldAllBe(element => !element.ToString().Contains("<masterKey", StringComparison.Ordinal));
        Protector(repository, TestSupport.Configuration()).Unprotect(token).ShouldBe("cookie");
    }

    [Test]
    public void AChangedSecret_StartsANewKeyInsteadOfFailing()
    {
        var repository = new InMemoryRepository();
        Protector(repository, TestSupport.Configuration()).Protect("cookie");
        var rotated = Protector(repository, TestSupport.Configuration(new Dictionary<string, string?>
        {
            ["Identity:KeyEncryptionSecret"] = "a-replacement-key-encryption-secret-32+"
        }));

        // Old cookies become invalid (everyone signs in again), but protection keeps working.
        rotated.Unprotect(rotated.Protect("new cookie")).ShouldBe("new cookie");
    }

    private static IDataProtector Protector(IXmlRepository repository, IConfiguration configuration)
    {
        var services = new ServiceCollection();
        services.AddSingleton(configuration);
        services.AddSingleton<IdentityKeyProtector>();
        services.AddDataProtection().SetApplicationName("cpnucleo-identity-tests");
        services.Configure<KeyManagementOptions>(options => options.XmlRepository = repository);
        services.AddSingleton<Microsoft.Extensions.Options.IConfigureOptions<KeyManagementOptions>>(provider =>
            new Microsoft.Extensions.Options.ConfigureOptions<KeyManagementOptions>(options =>
                options.XmlEncryptor = new KeyEncryptionXmlEncryptor(provider.GetRequiredService<IdentityKeyProtector>())));
        return services.BuildServiceProvider().GetDataProtector("tests");
    }

    private sealed class InMemoryRepository : IXmlRepository
    {
        public List<XElement> Elements { get; } = [];
        public IReadOnlyCollection<XElement> GetAllElements() => Elements.ToList();
        public void StoreElement(XElement element, string friendlyName) => Elements.Add(new XElement(element));
    }
}
