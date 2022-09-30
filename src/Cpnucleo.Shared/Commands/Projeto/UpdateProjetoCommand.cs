namespace Cpnucleo.Shared.Commands.Projeto;

public sealed record UpdateProjetoCommand(Guid Id, string Nome, Guid IdSistema) : BaseCommand, IRequest<OperationResult>;