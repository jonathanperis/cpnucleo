namespace Cpnucleo.Shared.Commands.Sistema;

public record RemoveSistemaCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;