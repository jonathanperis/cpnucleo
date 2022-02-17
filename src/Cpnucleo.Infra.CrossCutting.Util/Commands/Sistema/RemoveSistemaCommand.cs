namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema;

public class RemoveSistemaCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
}
