namespace Cpnucleo.Shared.Commands.Projeto;

public sealed record CreateProjetoCommand(Guid Id, string Nome, Guid IdSistema) : BaseCommand, IRequest<OperationResult>;