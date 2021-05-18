namespace Cpnucleo.Domain.Commands.Responses.RecursoProjeto
{
    public class CreateRecursoProjetoResponse
    {
        public OperationResult Status { get; set; }
        public Domain.Entities.RecursoProjeto RecursoProjeto { get; set; }
    }
}
