using Microsoft.Extensions.Configuration;

namespace dotnet_cpnucleo_pages.Repository2
{
    public class Context 
    {
        private string _connectionString;

        protected string ConnectionString => _connectionString;

        public Context(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
    }
}
