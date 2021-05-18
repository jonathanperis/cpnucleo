namespace Cpnucleo.Domain.Commands.Responses.Impedimento
{
    public class CreateImpedimentoResponse
    {
        public OperationResult Status { get; set; }
        public Domain.Entities.Impedimento Impedimento { get; set; }
    }
}
