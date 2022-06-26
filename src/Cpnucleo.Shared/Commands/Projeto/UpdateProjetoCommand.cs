namespace Cpnucleo.Shared.Commands.Projeto;

public record UpdateProjetoCommand(Guid Id, string Nome, Guid IdSistema) : BaseCommand, IRequest<OperationResult>;