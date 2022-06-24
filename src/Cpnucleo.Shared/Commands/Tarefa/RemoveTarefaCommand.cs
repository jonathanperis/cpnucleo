using Cpnucleo.Shared.Commands;
using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Shared.Commands.Tarefa;

public record RemoveTarefaCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;