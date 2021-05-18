namespace Cpnucleo.Domain.Commands.Responses.RecursoTarefa
{
    public class CreateRecursoTarefaResponse
    {
        public OperationResult Status { get; set; }
        public Domain.Entities.RecursoTarefa RecursoTarefa { get; set; }
    }
}
