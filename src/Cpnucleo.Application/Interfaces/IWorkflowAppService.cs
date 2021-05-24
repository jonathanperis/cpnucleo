using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System.Threading.Tasks;

namespace Cpnucleo.Application.Interfaces
{
    public interface IWorkflowAppService : IGenericAppService<WorkflowViewModel>
    {
        Task<int> GetQuantidadeColunasAsync();
        string GetTamanhoColuna(int colunas);
    }
}
