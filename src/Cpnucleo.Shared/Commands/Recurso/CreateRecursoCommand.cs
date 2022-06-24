using Cpnucleo.Shared.Commands;
using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Shared.Commands.Recurso;

public record CreateRecursoCommand(Guid Id, string Nome, string Login, string Senha) : BaseCommand, IRequest<OperationResult>;