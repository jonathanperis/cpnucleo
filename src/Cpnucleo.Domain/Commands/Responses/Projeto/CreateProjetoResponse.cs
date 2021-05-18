namespace Cpnucleo.Domain.Commands.Responses.Projeto
{
    public class CreateProjetoResponse
    {
        public OperationResult Status { get; set; }
        public Domain.Entities.Projeto Projeto { get; set; }
    }
}
