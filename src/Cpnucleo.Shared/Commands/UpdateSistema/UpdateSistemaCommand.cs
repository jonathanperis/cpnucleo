namespace Cpnucleo.Shared.Commands.UpdateSistema;

public sealed record UpdateSistemaCommand(Guid Id, string Nome, string Descricao) : BaseCommand, IRequest<OperationResult>;