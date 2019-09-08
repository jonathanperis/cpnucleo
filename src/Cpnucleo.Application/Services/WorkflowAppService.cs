using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using System;
using System.Linq;

namespace Cpnucleo.Application.Services
{
    public class WorkflowAppService : AppService<Workflow, WorkflowViewModel>, IWorkflowAppService
    {
        public WorkflowAppService(IMapper mapper, IRepository<Workflow> repository)
            : base(mapper, repository)
        {

        }

        public IQueryable<WorkflowViewModel> ListarTarefasWorkflow()
        {
            throw new NotImplementedException();
        }
    }
}
