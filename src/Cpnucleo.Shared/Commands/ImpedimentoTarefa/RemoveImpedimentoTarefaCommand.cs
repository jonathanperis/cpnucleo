using Cpnucleo.Shared.Commands;
using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Shared.Commands.ImpedimentoTarefa;

public record RemoveImpedimentoTarefaCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;