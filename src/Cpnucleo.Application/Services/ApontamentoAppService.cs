using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Services;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Application.Services
{
    internal class ApontamentoAppService : CrudAppService<Apontamento, ApontamentoViewModel>, IApontamentoAppService
    {
        private readonly IApontamentoService _apontamentoService;

        public ApontamentoAppService(IMapper mapper, IApontamentoService apontamentoService)
            : base(mapper, apontamentoService)
        {
            _apontamentoService = apontamentoService;
        }

        public IEnumerable<ApontamentoViewModel> ListarPorRecurso(Guid idRecurso)
        {
            return _mapper.Map<IEnumerable<ApontamentoViewModel>>(_apontamentoService.ListarPorRecurso(idRecurso));
        }

        public int ObterTotalHorasPorRecurso(Guid idRecurso, Guid idTarefa)
        {
            return _apontamentoService.ObterTotalHorasPorRecurso(idRecurso, idTarefa);
        }
    }
}
