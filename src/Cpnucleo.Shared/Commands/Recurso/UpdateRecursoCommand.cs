namespace Cpnucleo.Shared.Commands.Recurso;

public sealed record UpdateRecursoCommand(Guid Id, string Nome, string Senha) : BaseCommand, IRequest<OperationResult>;