using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Shared.Commands.Recurso;

public record UpdateRecursoCommand(Guid Id, string Nome, string Senha) : BaseCommand, IRequest<OperationResult>;