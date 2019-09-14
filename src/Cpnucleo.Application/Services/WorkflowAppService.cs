using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpnucleo.Application.Services
{
    public class WorkflowAppService : AppService<Workflow, WorkflowViewModel>, IWorkflowAppService
    {
        protected readonly IWorkflowRepository _workflowRepository;

        public WorkflowAppService(IMapper mapper, IRepository<Workflow> repository, IUnitOfWork unitOfWork, IWorkflowRepository workflowRepository)
            : base(mapper, repository, unitOfWork)
        {
            _workflowRepository = workflowRepository;
        }

        public IEnumerable<WorkflowViewModel> ListarPorTarefa()
        {
            var listaWorkflow = _mapper.Map<IEnumerable<WorkflowViewModel>>(_workflowRepository.ListarPorTarefa());

            foreach (var item in listaWorkflow)
            {
                foreach (var tarefa in item.ListaTarefas)
                {
                    tarefa.HorasConsumidas = tarefa.ListaApontamentos.Sum(x => x.QtdHoras);
                    tarefa.HorasRestantes = tarefa.QtdHoras - tarefa.HorasConsumidas;

                    if (DateTime.Now.Date >= tarefa.DataInicio && DateTime.Now.Date <= tarefa.DataTermino)
                    {
                        tarefa.TipoTarefa.Element = "success-element";
                    }
                    else if (tarefa.ListaImpedimentos.Count(x => x.Ativo) > 0)
                    {
                        tarefa.TipoTarefa.Element = "warning-element";
                    }
                    else if (DateTime.Now.Date > tarefa.DataTermino && tarefa.PercentualConcluido != 100)
                    {
                        tarefa.TipoTarefa.Element = "danger-element";
                    }
                    else
                    {
                        tarefa.TipoTarefa.Element = "info-element";
                    }
                }
            }

            return listaWorkflow;
        }
    }
}
