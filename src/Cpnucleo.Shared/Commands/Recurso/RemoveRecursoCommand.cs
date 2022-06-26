namespace Cpnucleo.Shared.Commands.Recurso;

public record RemoveRecursoCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;