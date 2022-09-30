namespace Cpnucleo.Shared.Commands.Projeto;

public sealed record RemoveProjetoCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;