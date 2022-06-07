using Cpnucleo.Application.Common.Security;
using Cpnucleo.Application.Common.Security.Interfaces;

namespace Cpnucleo.Application.Test.Helpers;

public class CryptographyHelper
{
    public static ICryptographyManager GetInstance()
    {
        return new CryptographyManager();
    }
}
