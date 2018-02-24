using Microsoft.Extensions.Configuration;

namespace dotnet_cpnucleo_pages.Repository2.Sistema
{
    public class SistemaContext
    {
        private string _connectionString;

        protected string ConnectionString => _connectionString;

        public SistemaContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
    } 
}