using Cpnucleo.Shared.Commands;
using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Shared.Commands.TipoTarefa;

public record RemoveTipoTarefaCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;