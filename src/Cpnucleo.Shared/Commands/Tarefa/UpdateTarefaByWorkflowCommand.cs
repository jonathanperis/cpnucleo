using Cpnucleo.Shared.Commands;
using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Shared.Commands.Tarefa;

public record UpdateTarefaByWorkflowCommand(Guid Id, Guid IdWorkflow) : BaseCommand, IRequest<OperationResult>;