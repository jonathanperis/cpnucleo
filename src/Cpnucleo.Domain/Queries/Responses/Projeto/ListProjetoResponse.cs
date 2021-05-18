using System.Collections.Generic;

namespace Cpnucleo.Domain.Queries.Responses.Projeto
{
    public class ListProjetoResponse
    {
        public OperationResult Status { get; set; }
        public IEnumerable<Domain.Entities.Projeto> Projetos { get; set; }
    }
}
