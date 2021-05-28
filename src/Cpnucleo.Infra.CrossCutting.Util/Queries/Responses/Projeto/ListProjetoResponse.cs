using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System.Collections.Generic;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Projeto
{
    public class ListProjetoResponse
    {
        public OperationResult Status { get; set; }
        public IEnumerable<ProjetoViewModel> Projetos { get; set; }
    }
}
