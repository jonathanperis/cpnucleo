using Cpnucleo.Application.Common.Security.Interfaces;
using Cpnucleo.Infra.CrossCutting.Security;

namespace Cpnucleo.Application.Test.Helpers;

public class CryptographyHelper
{
    public static ICryptographyManager GetInstance()
    {
        return new CryptographyManager();
    }
}
