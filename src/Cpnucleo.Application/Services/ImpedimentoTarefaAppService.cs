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
    public class ImpedimentoTarefaAppService : CrudAppService<ImpedimentoTarefa, ImpedimentoTarefaViewModel>, IImpedimentoTarefaAppService
    {
        private readonly IImpedimentoTarefaService _impedimentoTarefaService;

        public ImpedimentoTarefaAppService(IMapper mapper, IImpedimentoTarefaService impedimentoTarefaService)
            : base(mapper, impedimentoTarefaService)
        {
            _impedimentoTarefaService = impedimentoTarefaService;
        }

        public IEnumerable<ImpedimentoTarefaViewModel> ListarPorTarefa(Guid idTarefa)
        {
            return _impedimentoTarefaService.ListarPorTarefa(idTarefa).ProjectTo<ImpedimentoTarefaViewModel>(_mapper.ConfigurationProvider).ToList();
        }
    }
}
