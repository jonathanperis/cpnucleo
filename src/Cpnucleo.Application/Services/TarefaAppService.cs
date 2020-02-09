using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpnucleo.Application.Services
{
    public class TarefaAppService : CrudAppService<Tarefa, TarefaViewModel>, ITarefaAppService
    {
        private readonly IWorkflowAppService _workflowAppService;
        private readonly IApontamentoAppService _apontamentoAppService;
        private readonly IImpedimentoTarefaAppService _impedimentoTarefaAppService;

        public TarefaAppService(IMapper mapper, ICrudRepository<Tarefa> repository, IUnitOfWork unitOfWork, IWorkflowAppService workflowAppService, IApontamentoAppService apontamentoAppService, IImpedimentoTarefaAppService impedimentoTarefaAppService)
            : base(mapper, repository, unitOfWork)
        {
            _workflowAppService = workflowAppService;
            _apontamentoAppService = apontamentoAppService;
            _impedimentoTarefaAppService = impedimentoTarefaAppService;
        }

        public new IEnumerable<TarefaViewModel> Listar()
        {
            IEnumerable<TarefaViewModel> lista = base.Listar();

            string tamanhoColuna = _workflowAppService.ObterTamanhoColuna();

            foreach (TarefaViewModel item in lista)
            {
                item.Workflow.TamanhoColuna = tamanhoColuna;
                item.HorasConsumidas = _apontamentoAppService.ObterTotalHorasPorRecurso(item.IdRecurso, item.Id);
                item.HorasRestantes = item.QtdHoras - item.HorasConsumidas;

                if (_impedimentoTarefaAppService.ListarPorTarefa(item.Id).Count() > 0)
                {
                    item.TipoTarefa.Element = "warning-element";
                }
                else if (DateTime.Now.Date >= item.DataInicio && DateTime.Now.Date <= item.DataTermino)
                {
                    item.TipoTarefa.Element = "success-element";
                }
                else if (DateTime.Now.Date > item.DataTermino)
                {
                    item.TipoTarefa.Element = "danger-element";
                }
                else
                {
                    item.TipoTarefa.Element = "info-element";
                }
            }

            return lista;
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
