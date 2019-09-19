namespace Cpnucleo.Infra.CrossCutting.Configuration.Interfaces
{
    public interface ISystemConfiguration
    {
        /// <summary>
        /// Connection to the Azure Database.
        /// </summary>
        string ConnectionString { get; }
    }
}
