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
    public class ApontamentoAppService : AppService<Apontamento, ApontamentoViewModel>, IApontamentoAppService
    {
        protected readonly IApontamentoRepository _apontamentoRepository;

        public ApontamentoAppService(IMapper mapper, IRepository<Apontamento> repository, IApontamentoRepository apontamentoRepository)
            : base(mapper, repository)
        {
            _apontamentoRepository = apontamentoRepository;
        }

        public void ApontarHoras(ApontamentoViewModel apontamento)
        {
            throw new System.NotImplementedException();
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
