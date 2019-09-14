using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Application.Services
{
    public class RecursoTarefaAppService : AppService<RecursoTarefa, RecursoTarefaViewModel>, IRecursoTarefaAppService
    {
        protected readonly IRecursoTarefaRepository _recursoTarefaRepository;
        protected readonly IApontamentoAppService _apontamentoAppService;

        public RecursoTarefaAppService(IMapper mapper, IRepository<RecursoTarefa> repository, IUnitOfWork unitOfWork, IRecursoTarefaRepository recursoTarefaRepository, IApontamentoAppService apontamentoAppService)
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
            }

            return listaRecursoTarefa;
        }

        public IEnumerable<RecursoTarefaViewModel> ListarPorTarefa(Guid idTarefa)
        {
            return _mapper.Map<IEnumerable<RecursoTarefaViewModel>>(_recursoTarefaRepository.ListarPorTarefa(idTarefa));
        }
    }
}
