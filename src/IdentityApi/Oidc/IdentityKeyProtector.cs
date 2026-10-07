using System.Security.Cryptography;
using System.Text;

namespace IdentityApi.Oidc;

/// <summary>
/// Encrypts identity key material at rest (signing keys, Data Protection keys) with AES-256-GCM.
/// The key-encryption key is derived (HKDF-SHA256) from <c>Identity:KeyEncryptionSecret</c>, falling
/// back to the existing <c>Jwt:SigningKey</c> secret so deployments need no new variable. The purpose
/// string is authenticated, so ciphertext for one purpose can't be replayed as another.
/// </summary>
public sealed class IdentityKeyProtector
{
    public const int MinimumSecretBytes = 32;
    private const byte FormatVersion = 1;
    private const int NonceSize = 12;
    private const int TagSize = 16;
    private readonly byte[] key;

    public IdentityKeyProtector(IConfiguration configuration)
    {
        var secret = configuration["Identity:KeyEncryptionSecret"] ?? configuration["Jwt:SigningKey"]
            ?? throw new InvalidOperationException("Identity:KeyEncryptionSecret (or Jwt:SigningKey) is required to protect identity keys.");
        var secretBytes = Encoding.UTF8.GetBytes(secret);
        if (secretBytes.Length < MinimumSecretBytes)
            throw new InvalidOperationException($"The identity key-encryption secret must be at least {MinimumSecretBytes} bytes.");
        key = HKDF.DeriveKey(HashAlgorithmName.SHA256, secretBytes, 32,
            salt: Encoding.UTF8.GetBytes("cpnucleo-identity"), info: Encoding.UTF8.GetBytes("key-encryption-v1"));
    }

    public byte[] Protect(ReadOnlySpan<byte> plaintext, string purpose)
    {
        var output = new byte[1 + NonceSize + TagSize + plaintext.Length];
        output[0] = FormatVersion;
        var nonce = output.AsSpan(1, NonceSize);
        RandomNumberGenerator.Fill(nonce);
        using var aes = new AesGcm(key, TagSize);
        aes.Encrypt(nonce, plaintext, output.AsSpan(1 + NonceSize + TagSize), output.AsSpan(1 + NonceSize, TagSize),
            Encoding.UTF8.GetBytes(purpose));
        return output;
    }

    public byte[] Unprotect(ReadOnlySpan<byte> protectedData, string purpose)
    {
        if (protectedData.Length < 1 + NonceSize + TagSize || protectedData[0] != FormatVersion)
            throw new CryptographicException("Unsupported protected key format.");
        var plaintext = new byte[protectedData.Length - 1 - NonceSize - TagSize];
        using var aes = new AesGcm(key, TagSize);
        aes.Decrypt(protectedData.Slice(1, NonceSize), protectedData[(1 + NonceSize + TagSize)..],
            protectedData.Slice(1 + NonceSize, TagSize), plaintext, Encoding.UTF8.GetBytes(purpose));
        return plaintext;
    }
}
