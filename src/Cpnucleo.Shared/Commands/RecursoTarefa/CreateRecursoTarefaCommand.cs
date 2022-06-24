using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Shared.Commands.RecursoTarefa;

public record CreateRecursoTarefaCommand(Guid Id, Guid IdRecurso, Guid IdTarefa) : BaseCommand, IRequest<OperationResult>;