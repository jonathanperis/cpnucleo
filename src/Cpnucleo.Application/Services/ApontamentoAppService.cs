using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Repositories;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpnucleo.Application.Services
{
    public class ApontamentoAppService : CrudAppService<Apontamento, ApontamentoViewModel>, IApontamentoAppService
    {
        private readonly IApontamentoRepository _apontamentoRepository;

        public ApontamentoAppService(IMapper mapper, IApontamentoRepository repository, IUnitOfWork unitOfWork, IApontamentoRepository apontamentoRepository)
            : base(mapper, repository, unitOfWork)
        {
            _apontamentoRepository = apontamentoRepository;
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
