namespace Domain.Common.Security;

internal abstract class CryptographyManager
{
    internal static void CryptPbkdf2(string? item, out string itemCrypt, out string salt)
    {
        if (string.IsNullOrWhiteSpace(item))
        {
            itemCrypt = string.Empty;
            salt = string.Empty;
            return;
        }

        // Generate a random salt
        byte[] saltBytes = RandomNumberGenerator.GetBytes(48);
        
        // Derive the key using PBKDF2
        byte[] itemBytes = Rfc2898DeriveBytes.Pbkdf2(item, saltBytes, 1000, HashAlgorithmName.SHA1, 48);

        salt = Convert.ToBase64String(saltBytes);
        itemCrypt = Convert.ToBase64String(itemBytes);
    }

    internal static bool VerifyPbkdf2(string? item, string? itemCrypt, string? salt)
    {
        if (string.IsNullOrWhiteSpace(item) || string.IsNullOrWhiteSpace(itemCrypt) || string.IsNullOrWhiteSpace(salt))
        {
            return false;
        }

        var saltBytes = Convert.FromBase64String(salt);
        var itemBytes = Convert.FromBase64String(itemCrypt);

        // Derive the key using PBKDF2 with the stored salt
        byte[] derivedBytes = Rfc2898DeriveBytes.Pbkdf2(item, saltBytes, 1000, HashAlgorithmName.SHA1, 48);

        return derivedBytes.SequenceEqual(itemBytes);
    }
}