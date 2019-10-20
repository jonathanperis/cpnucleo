using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpnucleo.Application.Services
{
    public class ApontamentoAppService : CrudAppService<Apontamento, ApontamentoViewModel>, IApontamentoAppService
    {
        protected readonly IApontamentoRepository _apontamentoRepository;
        protected readonly ITarefaAppService _tarefaAppService;

        public ApontamentoAppService(IMapper mapper, ICrudRepository<Apontamento> repository, IUnitOfWork unitOfWork, IApontamentoRepository apontamentoRepository, ITarefaAppService tarefaAppService)
            : base(mapper, repository, unitOfWork)
        {
            _apontamentoRepository = apontamentoRepository;
            _tarefaAppService = tarefaAppService;
        }

        public new bool Incluir(ApontamentoViewModel apontamento)
        {
            _repository.Incluir(_mapper.Map<Apontamento>(apontamento));

            return _tarefaAppService.AlterarPorPercentualConcluido(apontamento.IdTarefa, apontamento.PercentualConcluido);
        }

        public IEnumerable<ApontamentoViewModel> ListarPorRecurso(Guid idRecurso)
        {
            return _apontamentoRepository.ListarPorRecurso(idRecurso).ProjectTo<ApontamentoViewModel>(_mapper.ConfigurationProvider).ToList();
        }

        public int ObterTotalHorasPorRecurso(Guid idRecurso, Guid idTarefa)
        {
            return _apontamentoRepository.ObterTotalHorasPorRecurso(idRecurso, idTarefa);
        }
    }
}
