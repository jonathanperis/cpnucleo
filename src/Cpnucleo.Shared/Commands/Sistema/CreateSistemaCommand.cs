namespace Cpnucleo.Shared.Commands.Sistema;

public sealed record CreateSistemaCommand(Guid Id, string Nome, string Descricao) : BaseCommand, IRequest<OperationResult>;