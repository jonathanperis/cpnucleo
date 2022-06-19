namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.Sistema;

public record RemoveSistemaCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;