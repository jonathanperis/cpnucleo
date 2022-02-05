namespace Cpnucleo.Application.Commands.Sistema.RemoveSistema;

public class RemoveSistemaCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
}
