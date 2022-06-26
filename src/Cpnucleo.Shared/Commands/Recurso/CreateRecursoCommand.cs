namespace Cpnucleo.Shared.Commands.Recurso;

public record CreateRecursoCommand(Guid Id, string Nome, string Login, string Senha) : BaseCommand, IRequest<OperationResult>;