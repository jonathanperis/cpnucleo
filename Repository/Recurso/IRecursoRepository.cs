namespace dotnet_cpnucleo_pages.Repository.Recurso
{
    public interface IRecursoRepository : IRepository<RecursoItem>
    {
        RecursoItem ValidarRecurso(string usuario, string senha, out bool valido);
    }
}