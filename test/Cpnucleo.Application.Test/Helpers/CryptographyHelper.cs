using Cpnucleo.Infra.CrossCutting.Security;
using Cpnucleo.Infra.CrossCutting.Security.Interfaces;

namespace Cpnucleo.Application.Test.Helpers;

public class CryptographyHelper
{
    public static ICryptographyManager GetInstance()
    {
        return new CryptographyManager();
    }
}
