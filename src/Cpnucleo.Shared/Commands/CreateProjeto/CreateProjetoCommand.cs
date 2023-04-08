namespace Cpnucleo.Shared.Commands.CreateProjeto;

public sealed record CreateProjetoCommand(Guid Id, string Nome, Guid IdSistema) : BaseCommand, IRequest<OperationResult>;