using Cpnucleo.Infra.CrossCutting.Security;
using Cpnucleo.Infra.CrossCutting.Security.Interfaces;

namespace Cpnucleo.Application.Test
{
    class CryptographyHelper
    {
        public static ICryptographyManager GetInstance()
        {
            return new CryptographyManager();
        }
    }
}
