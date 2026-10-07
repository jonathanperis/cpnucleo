using System.Text;
using System.Xml.Linq;
using Dapper;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;

namespace IdentityApi.Oidc;

/// <summary>
/// Keeps ASP.NET Core Data Protection keys (identity cookies, sign-in state) in PostgreSQL instead of
/// the container's read-only file system, so restarts don't sign everyone out. Keys are encrypted by
/// <see cref="KeyEncryptionXmlEncryptor"/> before they reach the repository.
/// </summary>
public sealed class DataProtectionKeyStore(NpgsqlDataSource dataSource) : IXmlRepository
{
    public IReadOnlyCollection<XElement> GetAllElements()
    {
        using var connection = dataSource.OpenConnection();
        return connection.Query<string>("""SELECT "Xml" FROM "IdentityDataProtectionKeys" ORDER BY "Id" """)
            .Select(xml => XElement.Parse(xml)).ToList();
    }

    public void StoreElement(XElement element, string friendlyName)
    {
        using var connection = dataSource.OpenConnection();
        connection.Execute("""INSERT INTO "IdentityDataProtectionKeys" ("FriendlyName", "Xml") VALUES (@friendlyName, @xml)""",
            new { friendlyName, xml = element.ToString(SaveOptions.DisableFormatting) });
    }
}

/// <summary>Encrypts Data Protection key XML with the identity key-encryption key (AES-256-GCM).</summary>
public sealed class KeyEncryptionXmlEncryptor(IdentityKeyProtector protector) : IXmlEncryptor
{
    internal const string Purpose = "data-protection-key";

    public EncryptedXmlInfo Encrypt(XElement plaintextElement)
    {
        var ciphertext = protector.Protect(Encoding.UTF8.GetBytes(plaintextElement.ToString(SaveOptions.DisableFormatting)), Purpose);
        return new EncryptedXmlInfo(new XElement("encryptedKey", Convert.ToBase64String(ciphertext)), typeof(KeyEncryptionXmlDecryptor));
    }
}

/// <summary>Data Protection creates decryptors by type with an <see cref="IServiceProvider"/> constructor.</summary>
public sealed class KeyEncryptionXmlDecryptor(IServiceProvider services) : IXmlDecryptor
{
    public XElement Decrypt(XElement encryptedElement) =>
        XElement.Parse(Encoding.UTF8.GetString(services.GetRequiredService<IdentityKeyProtector>()
            .Unprotect(Convert.FromBase64String(encryptedElement.Value), KeyEncryptionXmlEncryptor.Purpose)));
}
