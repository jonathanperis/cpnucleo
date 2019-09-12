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
        protected readonly ITarefaAppService _tarefaAppService;

        public ApontamentoAppService(IMapper mapper, IRepository<Apontamento> repository, IUnitOfWork unitOfWork, IApontamentoRepository apontamentoRepository, ITarefaAppService tarefaAppService)
            : base(mapper, repository, unitOfWork)
        {
            _apontamentoRepository = apontamentoRepository;
            _tarefaAppService = tarefaAppService;
        }

        public void ApontarHoras(ApontamentoViewModel apontamento)
        {
            Incluir(apontamento);

            var tarefa = _tarefaAppService.Consultar(apontamento.IdTarefa);
            tarefa.PercentualConcluido = apontamento.PercentualConcluido;

            _tarefaAppService.Alterar(tarefa);

            //_unitOfWork.Commit();
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
