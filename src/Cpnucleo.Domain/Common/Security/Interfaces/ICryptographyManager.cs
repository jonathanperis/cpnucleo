namespace Cpnucleo.Domain.Common.Security.Interfaces;

public interface ICryptographyManager
{
    void CryptPbkdf2(string item, out string itemCrypt, out string salt);

    bool VerifyPbkdf2(string item, string itemCrypt, string salt);
}