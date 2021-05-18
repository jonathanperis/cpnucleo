namespace Cpnucleo.Domain.Queries.Responses.Tarefa
{
    public class GetTarefaResponse
    {
        public OperationResult Status { get; set; }
        public Domain.Entities.Tarefa Tarefa { get; set; }
    }
}
