namespace Cpnucleo.Domain.Commands.Responses.Tarefa
{
    public class CreateTarefaResponse
    {
        public OperationResult Status { get; set; }
        public Domain.Entities.Tarefa Tarefa { get; set; }
    }
}
