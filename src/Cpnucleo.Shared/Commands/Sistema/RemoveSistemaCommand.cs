namespace Cpnucleo.Shared.Commands.Sistema;

public sealed record RemoveSistemaCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;