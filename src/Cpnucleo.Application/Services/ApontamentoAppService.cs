using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Application.Services
{
    public class ApontamentoAppService : AppService<Apontamento, ApontamentoViewModel>, IApontamentoAppService
    {
        protected readonly IApontamentoRepository _apontamentoRepository;
        protected readonly ITarefaRepository _tarefaRepository;

        public ApontamentoAppService(IMapper mapper, IRepository<Apontamento> repository, IApontamentoRepository apontamentoRepository, ITarefaRepository tarefaRepository)
            : base(mapper, repository)
        {
            _apontamentoRepository = apontamentoRepository;
            _tarefaRepository = tarefaRepository;
        }

        public void ApontarHoras(ApontamentoViewModel apontamento)
        {
            Incluir(apontamento);

            var tarefa = _mapper.Map<TarefaViewModel>(_tarefaRepository.Consultar(apontamento.IdTarefa));
            tarefa.PercentualConcluido = apontamento.PercentualConcluido;

            _tarefaRepository.Alterar(_mapper.Map<Tarefa>(tarefa));
        }

        public IEnumerable<ApontamentoViewModel> ListarPoridRecurso(Guid idRecurso)
        {
            return _mapper.Map<IEnumerable<ApontamentoViewModel>>(_apontamentoRepository.ListarPoridRecurso(idRecurso));
        }

        public int ObterTotalHorasPoridRecurso(Guid idRecurso, Guid idTarefa)
        {
            return _apontamentoRepository.ObterTotalHorasPoridRecurso(idRecurso, idTarefa);
        }
    }
}
