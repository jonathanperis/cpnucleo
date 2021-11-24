namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema;

public class RemoveSistemaCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
}