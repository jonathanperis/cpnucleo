using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Shared.Commands.RecursoTarefa;

public record RemoveRecursoTarefaCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;