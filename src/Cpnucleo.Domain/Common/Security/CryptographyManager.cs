using System.Security.Cryptography;

namespace Cpnucleo.Domain.Common.Security;

internal sealed class CryptographyManager
{
    internal static void CryptPbkdf2(string item, out string itemCrypt, out string salt)
    {
        using Rfc2898DeriveBytes deriveBytes = new(item, 48);

        byte[] saltBytes = deriveBytes.Salt;
        byte[] itemBytes = deriveBytes.GetBytes(48);

        salt = Convert.ToBase64String(saltBytes);
        itemCrypt = Convert.ToBase64String(itemBytes);
    }

    internal static bool VerifyPbkdf2(string item, string itemCrypt, string salt)
    {
        byte[] saltBytes = Convert.FromBase64String(salt);
        byte[] itemBytes = Convert.FromBase64String(itemCrypt);

        using Rfc2898DeriveBytes deriveBytes = new(item, saltBytes);
        
        byte[] newItem = deriveBytes.GetBytes(48);

        if (!newItem.SequenceEqual(itemBytes))
        {
            return false;
        }

        return true;
    }
}