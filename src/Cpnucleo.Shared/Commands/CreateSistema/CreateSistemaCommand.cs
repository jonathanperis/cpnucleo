namespace Cpnucleo.Shared.Commands.CreateSistema;

public sealed record CreateSistemaCommand(string Nome, string Descricao) : BaseCommand, IRequest<OperationResult>;