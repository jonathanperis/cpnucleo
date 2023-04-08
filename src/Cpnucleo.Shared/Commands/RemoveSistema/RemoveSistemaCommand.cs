namespace Cpnucleo.Shared.Commands.RemoveSistema;

public sealed record RemoveSistemaCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;