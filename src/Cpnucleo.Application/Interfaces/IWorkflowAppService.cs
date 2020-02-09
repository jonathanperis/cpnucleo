using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Application.Interfaces
{
    public interface IWorkflowAppService : ICrudAppService<WorkflowViewModel>
    {
        int ObterQuantidadeColunas();

        string ObterTamanhoColuna(int quantidadeColunas);
    }
}
