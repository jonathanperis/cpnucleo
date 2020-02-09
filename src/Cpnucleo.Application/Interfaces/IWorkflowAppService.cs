using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System.Collections.Generic;

namespace Cpnucleo.Application.Interfaces
{
    public interface IWorkflowAppService : ICrudAppService<WorkflowViewModel>
    {
        string ObterTamanhoColuna();
    }
}
