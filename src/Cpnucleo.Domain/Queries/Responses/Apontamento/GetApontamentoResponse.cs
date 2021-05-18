namespace Cpnucleo.Domain.Queries.Responses.Apontamento
{
    public class GetApontamentoResponse
    {
        public OperationResult Status { get; set; }
        public Domain.Entities.Apontamento Apontamento { get; set; }
    }
}
