namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.Sistema;

public class RemoveSistemaCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
}
