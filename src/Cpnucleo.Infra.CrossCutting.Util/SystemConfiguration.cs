using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Cpnucleo.Infra.CrossCutting.Util
{
    public class SystemConfiguration : ISystemConfiguration
    {
        private readonly IConfiguration _configuration;

        public SystemConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ConnectionString
        {
            get
            {
                string value = _configuration.GetConnectionString("DefaultConnection");

                if (value != null)
                {
                    return value;
                }

                return string.Empty;
            }
        }

        public string JwtKey
        {
            get
            {
                string value = _configuration.GetSection("Jwt")["Key"];

                if (value != null)
                {
                    return value;
                }

                return string.Empty;
            }
        }

        public int JwtExpirationDate
        {
            get
            {
                int.TryParse(_configuration.GetSection("Jwt")["ExpirationDate"], out int value);

                return value;
            }
        }

        public string UrlCpnucleoApi
        {
            get
            {
                string value = _configuration.GetSection("AppSettings")["UrlCpnucleoApi"];

                if (value != null)
                {
                    return value;
                }

                return string.Empty;
            }
        }

        public string UrlCpnucleoGrpc
        {
            get
            {
                string value = _configuration.GetSection("AppSettings")["UrlCpnucleoGrpc"];

                if (value != null)
                {
                    return value;
                }

                return string.Empty;
            }
        }
    }
}
