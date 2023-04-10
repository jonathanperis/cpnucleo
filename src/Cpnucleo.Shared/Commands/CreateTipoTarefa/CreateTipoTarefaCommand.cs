namespace Cpnucleo.Shared.Commands.CreateTipoTarefa;

public sealed record CreateTipoTarefaCommand(string Nome, string Image, Guid Id = default) : BaseCommand, IRequest<OperationResult>;