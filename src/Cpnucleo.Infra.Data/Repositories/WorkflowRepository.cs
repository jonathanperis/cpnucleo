using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Repositories;
using Cpnucleo.Infra.Data.Context;
using System.Linq;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.Data.Repositories
{
    internal class WorkflowRepository : GenericRepository<Workflow>, IWorkflowRepository
    {
        public WorkflowRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public async Task<int> GetQuantidadeColunasAsync()
        {
            System.Collections.Generic.IEnumerable<Workflow> result = await AllAsync(true);

            return result.Count();
        }

        public string GetTamanhoColuna(int colunas)
        {
            colunas = colunas == 1 ? 2 : colunas;

            int i = 12 / colunas;
            return i.ToString();
        }
    }
}
