namespace Cpnucleo.Shared.Commands.CreateProjeto;

public sealed record CreateProjetoCommand(string Nome, Guid IdSistema, Guid Id = default) : BaseCommand, IRequest<OperationResult>;