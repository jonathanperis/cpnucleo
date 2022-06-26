namespace Cpnucleo.Shared.Commands.Projeto;

public record CreateProjetoCommand(Guid Id, string Nome, Guid IdSistema) : BaseCommand, IRequest<OperationResult>;