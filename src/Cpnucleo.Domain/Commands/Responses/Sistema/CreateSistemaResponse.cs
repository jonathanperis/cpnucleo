namespace Cpnucleo.Domain.Commands.Responses.Sistema
{
    public class CreateSistemaResponse
    {
        public OperationResult Status { get; set; }
        public Domain.Entities.Sistema Sistema { get; set; }
    }
}
