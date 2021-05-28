using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Util
{
    public interface IWorkflowGrpcService : IGenericGrpcService<WorkflowViewModel>
    {
        Task<int> GetQuantidadeColunasAsync();
        string GetTamanhoColuna(int colunas);
    }
}
