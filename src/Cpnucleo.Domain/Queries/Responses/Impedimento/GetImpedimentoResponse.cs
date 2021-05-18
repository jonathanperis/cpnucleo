namespace Cpnucleo.Domain.Queries.Responses.Impedimento
{
    public class GetImpedimentoResponse
    {
        public OperationResult Status { get; set; }
        public Domain.Entities.Impedimento Impedimento { get; set; }
    }
}
