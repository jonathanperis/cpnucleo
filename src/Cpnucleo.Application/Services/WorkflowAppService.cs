using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Services;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Application.Services
{
    internal class WorkflowAppService : CrudAppService<Workflow, WorkflowViewModel>, IWorkflowAppService
    {
        public WorkflowAppService(IMapper mapper, IWorkflowService service) 
            : base(mapper, service)
        {
        }
    }
}
