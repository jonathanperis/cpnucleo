namespace Cpnucleo.Shared.Commands.Sistema;

public sealed record UpdateSistemaCommand(Guid Id, string Nome, string Descricao) : BaseCommand, IRequest<OperationResult>;