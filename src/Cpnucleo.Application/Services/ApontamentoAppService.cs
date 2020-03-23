using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Services;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpnucleo.Application.Services
{
    public class ApontamentoAppService : CrudAppService<Apontamento, ApontamentoViewModel>, IApontamentoAppService
    {
        private readonly IApontamentoService _apontamentoService;

        public ApontamentoAppService(IMapper mapper, IApontamentoService apontamentoService)
            : base(mapper, apontamentoService)
        {
            _apontamentoService = apontamentoService;
        }

        public IEnumerable<ApontamentoViewModel> ListarPorRecurso(Guid idRecurso)
        {
            return _apontamentoService.ListarPorRecurso(idRecurso).ProjectTo<ApontamentoViewModel>(_mapper.ConfigurationProvider).ToList();
        }

        public int ObterTotalHorasPorRecurso(Guid idRecurso, Guid idTarefa)
        {
            return _apontamentoService.ObterTotalHorasPorRecurso(idRecurso, idTarefa);
        }
    }
}
