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
    public class RecursoTarefaAppService : CrudAppService<RecursoTarefa, RecursoTarefaViewModel>, IRecursoTarefaAppService
    {
        protected readonly IRecursoTarefaRepository _recursoTarefaRepository;
        protected readonly IApontamentoAppService _apontamentoAppService;

        public RecursoTarefaAppService(IMapper mapper, ICrudRepository<RecursoTarefa> repository, IUnitOfWork unitOfWork, IRecursoTarefaRepository recursoTarefaRepository, IApontamentoAppService apontamentoAppService)
            : base(mapper, repository, unitOfWork)
        {
            _recursoTarefaRepository = recursoTarefaRepository;
            _apontamentoAppService = apontamentoAppService;
        }

        public IEnumerable<RecursoTarefaViewModel> ListarPorRecurso(Guid idRecurso)
        {
            IEnumerable<RecursoTarefaViewModel> listaRecursoTarefa = _mapper.Map<IEnumerable<RecursoTarefaViewModel>>(_recursoTarefaRepository.ListarPorRecurso(idRecurso));

            foreach (RecursoTarefaViewModel item in listaRecursoTarefa)
            {
                item.HorasUtilizadas = _apontamentoAppService.ObterTotalHorasPorRecurso(item.IdRecurso, item.IdTarefa);

                if (item.PercentualTarefa != null)
                {
                    double horasFracionadas = ((item.Tarefa.QtdHoras / 100.0) * item.PercentualTarefa.Value);
                    item.HorasDisponiveis = (int)(horasFracionadas - item.HorasUtilizadas);
                }

                if (item.Tarefa.ListaImpedimentos.Count(x => x.Ativo) > 0)
                {
                    item.Tarefa.TipoTarefa.Element = "warning-element";
                }
                else if (DateTime.Now.Date >= item.Tarefa.DataInicio && DateTime.Now.Date <= item.Tarefa.DataTermino)
                {
                    item.Tarefa.TipoTarefa.Element = "success-element";
                }
                else if (DateTime.Now.Date > item.Tarefa.DataTermino && item.Tarefa.PercentualConcluido != 100)
                {
                    item.Tarefa.TipoTarefa.Element = "danger-element";
                }
                else
                {
                    item.Tarefa.TipoTarefa.Element = "info-element";
                }
            }

            return listaRecursoTarefa;
        }

        public IEnumerable<RecursoTarefaViewModel> ListarPorTarefa(Guid idTarefa)
        {
            return _mapper.Map<IEnumerable<RecursoTarefaViewModel>>(_recursoTarefaRepository.ListarPorTarefa(idTarefa));
        }
    }
}
