using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Application.Interfaces
{
    public interface IWorkflowAppService : ICrudAppService<WorkflowViewModel>
    {
        string ObterTamanhoColuna();
    }
}
