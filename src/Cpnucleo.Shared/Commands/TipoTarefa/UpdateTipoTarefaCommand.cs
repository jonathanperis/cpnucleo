using Cpnucleo.Shared.Commands;
using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Shared.Commands.TipoTarefa;

public record UpdateTipoTarefaCommand(Guid Id, string Nome, string Image) : BaseCommand, IRequest<OperationResult>;