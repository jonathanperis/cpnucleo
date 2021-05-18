namespace Cpnucleo.Domain.Commands.Responses.Recurso
{
    public class CreateRecursoResponse
    {
        public OperationResult Status { get; set; }
        public Domain.Entities.Recurso Recurso { get; set; }
    }
}
