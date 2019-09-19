using Cpnucleo.Infra.CrossCutting.Configuration.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Cpnucleo.Infra.CrossCutting.Configuration
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
                    return value;

                return string.Empty;
            }
        }
    }
}
