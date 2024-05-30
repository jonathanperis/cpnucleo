namespace Domain.Common.Security;

internal sealed class CryptographyManager
{
    internal static void CryptPbkdf2(string item, out string itemCrypt, out string salt)
    {
        using Rfc2898DeriveBytes deriveBytes = new(item, 48, 1000, HashAlgorithmName.SHA1);

        var saltBytes = deriveBytes.Salt;
        var itemBytes = deriveBytes.GetBytes(48);

        salt = Convert.ToBase64String(saltBytes);
        itemCrypt = Convert.ToBase64String(itemBytes);
    }

    internal static bool VerifyPbkdf2(string item, string itemCrypt, string salt)
    {
        var saltBytes = Convert.FromBase64String(salt);
        var itemBytes = Convert.FromBase64String(itemCrypt);

        using Rfc2898DeriveBytes deriveBytes = new(item, saltBytes, 1000, HashAlgorithmName.SHA1);

        return deriveBytes.GetBytes(48).SequenceEqual(itemBytes);
    }
}