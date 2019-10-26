using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;

namespace Cpnucleo.Application.Services
{
    public class TarefaAppService : CrudAppService<Tarefa, TarefaViewModel>, ITarefaAppService
    {
        private readonly IWorkflowAppService _workflowAppService;

        public TarefaAppService(IMapper mapper, ICrudRepository<Tarefa> repository, IUnitOfWork unitOfWork, IWorkflowAppService workflowAppService)
            : base(mapper, repository, unitOfWork)
        {
            _workflowAppService = workflowAppService;
        }

        public bool AlterarPorPercentualConcluido(Guid idTarefa, int? percentualConcluido)
        {
            TarefaViewModel tarefa = Consultar(idTarefa);
            tarefa.PercentualConcluido = percentualConcluido;

            return Alterar(tarefa);
        }

        public bool AlterarPorWorkflow(Guid idTarefa, Guid idWorkflow)
        {
            lock (this)
            {
                TarefaViewModel tarefa = Consultar(idTarefa);
                WorkflowViewModel workflow = _workflowAppService.Consultar(idWorkflow);

                tarefa.IdWorkflow = idWorkflow;
                tarefa.Workflow = workflow;

                return Alterar(tarefa);
            }
        }
    }
}
