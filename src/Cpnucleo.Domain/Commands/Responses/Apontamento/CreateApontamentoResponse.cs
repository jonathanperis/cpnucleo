namespace Cpnucleo.Domain.Commands.Responses.Apontamento
{
    public class CreateApontamentoResponse
    {
        public OperationResult Status { get; set; }
        public Domain.Entities.Apontamento Apontamento { get; set; }
    }
}
