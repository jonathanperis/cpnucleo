using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System.Collections.Generic;

namespace Cpnucleo.Infra.CrossCutting.Communication.Interfaces
{
    public interface IWorkflowApiService : ICrudApiService<WorkflowViewModel>
    {
        IEnumerable<WorkflowViewModel> ListarPorTarefa(string token);
    }
}
