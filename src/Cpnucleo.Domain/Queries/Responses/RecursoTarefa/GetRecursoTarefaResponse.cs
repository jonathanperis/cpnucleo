namespace Cpnucleo.Domain.Queries.Responses.RecursoTarefa
{
    public class GetRecursoTarefaResponse
    {
        public OperationResult Status { get; set; }
        public Domain.Entities.RecursoTarefa RecursoTarefa { get; set; }
    }
}
