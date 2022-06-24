using Cpnucleo.Shared.Common.DTOs;
using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Shared.Queries.Projeto;

public record ListProjetoViewModel : BaseQuery
{
    public IEnumerable<ProjetoDTO> Projetos { get; set; }
    public OperationResult OperationResult { get; set; }
}
