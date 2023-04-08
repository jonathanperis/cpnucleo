namespace Cpnucleo.Shared.Commands.RemoveTarefa;

public sealed record RemoveTarefaCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;