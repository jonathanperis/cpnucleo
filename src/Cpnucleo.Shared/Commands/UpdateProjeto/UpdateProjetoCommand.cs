namespace Cpnucleo.Shared.Commands.UpdateProjeto;

public sealed record UpdateProjetoCommand(Guid Id, string Nome, Guid IdSistema) : BaseCommand, IRequest<OperationResult>;