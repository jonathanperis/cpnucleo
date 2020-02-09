using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System.Linq;

namespace Cpnucleo.Application.Services
{
    public class WorkflowAppService : CrudAppService<Workflow, WorkflowViewModel>, IWorkflowAppService
    {
        public WorkflowAppService(IMapper mapper, ICrudRepository<Workflow> repository, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {

        }

        public string ObterTamanhoColuna()
        {
            int qtdLista = Listar().Count();
            qtdLista = qtdLista == 1 ? 2 : qtdLista;

            int i = 12 / qtdLista;
            return i.ToString();
        }
    }
}
