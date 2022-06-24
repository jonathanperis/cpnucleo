namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.Recurso;

public record RemoveRecursoCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;