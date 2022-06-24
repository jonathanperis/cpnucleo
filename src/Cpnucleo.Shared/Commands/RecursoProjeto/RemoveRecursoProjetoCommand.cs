using Cpnucleo.Shared.Commands;
using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Shared.Commands.RecursoProjeto;

public record RemoveRecursoProjetoCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;