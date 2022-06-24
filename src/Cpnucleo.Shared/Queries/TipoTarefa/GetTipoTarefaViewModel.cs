using Cpnucleo.Shared.Common.DTOs;
using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Shared.Queries.TipoTarefa;

public record GetTipoTarefaViewModel : BaseQuery
{
    public TipoTarefaDTO TipoTarefa { get; set; }
    public OperationResult OperationResult { get; set; }
}
