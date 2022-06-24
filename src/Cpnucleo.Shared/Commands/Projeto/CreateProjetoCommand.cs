namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.Projeto;

public record CreateProjetoCommand(Guid Id, string Nome, Guid IdSistema) : BaseCommand, IRequest<OperationResult>;