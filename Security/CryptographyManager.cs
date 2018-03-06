using System;
using System.Linq;
using System.Security.Cryptography;

namespace dotnet_cpnucleo_pages.Security
{
    public static class CryptographyManager
    {
        public static void CryptPbkdf2(string item, out string itemCriptografado, out string salt)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(item, 48))
            {
                byte[] saltBytes = deriveBytes.Salt;
                byte[] itemBytes = deriveBytes.GetBytes(48);

                salt = Convert.ToBase64String(saltBytes);
                itemCriptografado = Convert.ToBase64String(itemBytes);
            }
        }

        public static bool VerifyPbkdf2(string item, string itemCriptografado, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            byte[] itemBytes = Convert.FromBase64String(itemCriptografado);

            using (var deriveBytes = new Rfc2898DeriveBytes(item, saltBytes))
            {
                byte[] newItem = deriveBytes.GetBytes(48);

                if (!newItem.SequenceEqual(itemBytes)) return false;
            }            

            return true;
        }
    }
}
