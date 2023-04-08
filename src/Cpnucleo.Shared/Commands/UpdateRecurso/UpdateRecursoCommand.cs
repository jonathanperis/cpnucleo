namespace Cpnucleo.Shared.Commands.UpdateRecurso;

public sealed record UpdateRecursoCommand(Guid Id, string Nome, string Senha) : BaseCommand, IRequest<OperationResult>;