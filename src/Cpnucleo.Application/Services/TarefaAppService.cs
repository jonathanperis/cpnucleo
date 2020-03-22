using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpnucleo.Application.Services
{
    public class TarefaAppService : CrudAppService<Tarefa, TarefaViewModel>, ITarefaAppService
    {
        private readonly ITarefaRepository _tarefarepository;
        private readonly IWorkflowAppService _workflowAppService;
        private readonly IApontamentoAppService _apontamentoAppService;
        private readonly IImpedimentoTarefaAppService _impedimentoTarefaAppService;

        public TarefaAppService(IMapper mapper, ITarefaRepository repository, IUnitOfWork unitOfWork, IWorkflowAppService workflowAppService, IApontamentoAppService apontamentoAppService, IImpedimentoTarefaAppService impedimentoTarefaAppService, ITarefaRepository tarefarepository)
            : base(mapper, repository, unitOfWork)
        {
            _tarefarepository = tarefarepository;
            _workflowAppService = workflowAppService;
            _apontamentoAppService = apontamentoAppService;
            _impedimentoTarefaAppService = impedimentoTarefaAppService;
        }

        public new IEnumerable<TarefaViewModel> Listar()
        {
            IEnumerable<TarefaViewModel> lista = base.Listar();

            return PreencherDadosAdicionais(lista);
        }

        public IEnumerable<TarefaViewModel> ListarPorRecurso(Guid idRecurso)
        {
            List<TarefaViewModel> lista = _tarefarepository.ListarPorRecurso(idRecurso).ProjectTo<TarefaViewModel>(_mapper.ConfigurationProvider).ToList();

            return PreencherDadosAdicionais(lista);
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

        private IEnumerable<TarefaViewModel> PreencherDadosAdicionais(IEnumerable<TarefaViewModel> lista)
        {
            int quantidadeColunas = _workflowAppService.ObterQuantidadeColunas();

            foreach (TarefaViewModel item in lista)
            {
                item.Workflow.TamanhoColuna = _workflowAppService.ObterTamanhoColuna(quantidadeColunas);
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
    }
}
