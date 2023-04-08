namespace Cpnucleo.Shared.Commands.RemoveProjeto;

public sealed record RemoveProjetoCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;