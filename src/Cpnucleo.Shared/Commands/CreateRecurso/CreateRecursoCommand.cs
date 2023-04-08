namespace Cpnucleo.Shared.Commands.CreateRecurso;

public sealed record CreateRecursoCommand(string Nome, string Login, string Senha) : BaseCommand, IRequest<OperationResult>;