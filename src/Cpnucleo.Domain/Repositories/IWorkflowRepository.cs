using Cpnucleo.Domain.Entities;
using System.Threading.Tasks;

namespace Cpnucleo.Domain.Repositories
{
    public interface IWorkflowRepository : IGenericRepository<Workflow>
    {
        Task<int> GetQuantidadeColunasAsync();

        string GetTamanhoColuna(int quantidadeColunas);
    }
}