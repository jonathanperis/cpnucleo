namespace Cpnucleo.Shared.Commands.CreateProjeto;

public sealed record CreateProjetoCommand(string Nome, Guid IdSistema) : BaseCommand, IRequest<OperationResult>;