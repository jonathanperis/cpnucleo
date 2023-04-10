namespace Cpnucleo.Shared.Commands.CreateSistema;

public sealed record CreateSistemaCommand(string Nome, string Descricao, Guid Id = default) : BaseCommand, IRequest<OperationResult>;