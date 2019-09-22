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

        public new bool Incluir(ApontamentoViewModel apontamento)
        {
            if (apontamento.Id == Guid.Empty)
            {
                apontamento.Id = Guid.NewGuid();
            }

            apontamento.DataInclusao = DateTime.Now;

            _repository.Incluir(_mapper.Map<Apontamento>(apontamento));

            return _tarefaAppService.AlterarPorPercentualConcluido(apontamento.IdTarefa, apontamento.PercentualConcluido);
        }

        public IEnumerable<ApontamentoViewModel> ListarPorRecurso(Guid idRecurso)
        {
            return _mapper.Map<IEnumerable<ApontamentoViewModel>>(_apontamentoRepository.ListarPorRecurso(idRecurso));
        }

        public int ObterTotalHorasPorRecurso(Guid idRecurso, Guid idTarefa)
        {
            return _apontamentoRepository.ObterTotalHorasPorRecurso(idRecurso, idTarefa);
        }
    }
}
